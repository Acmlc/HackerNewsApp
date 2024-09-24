using HackerNewsApp.Models;
using HackerNewsApp.Services;
using LazyCache;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNewsApp.Queries
{
    public class GetHackerNewsByNameHandler : IRequestHandler<GetHackerNewsByName, List<HackerNewsStory?>>
    {
        private IAppCache _cache;
        private readonly IHackerNewsService _hackerNewsService;

        private static MemoryCacheEntryOptions _cacheSettings => new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.UtcNow.AddMinutes(5)
        };
        public GetHackerNewsByNameHandler(IAppCache cache, IHackerNewsService hackerNewsService)
        {
            _cache = cache;
            _hackerNewsService = hackerNewsService;
        }
        public async Task<List<HackerNewsStory?>> Handle(GetHackerNewsByName request, CancellationToken token)
        {
            string? searchTerm = request.searchTerm;
            int pageNumber = request.pageNumber;
            int pageSize = request.pageSize;

            List<HackerNewsStory?> stories = new List<HackerNewsStory?>();

            try {
                var hackerNewsApiResponse = await _hackerNewsService.NewStoriesAsync();
                if (hackerNewsApiResponse.IsSuccessStatusCode)
                {
                    var hackerNewsIdResult = hackerNewsApiResponse.Content.ReadAsStringAsync().Result;

                    var storyIds = JsonConvert.DeserializeObject<List<int>>(hackerNewsIdResult);

                    if (storyIds != null && storyIds.Count > 0)
                    {
                        var tasks = storyIds.Select(GetStoryByIdAsync);
                        stories = (await Task.WhenAll(tasks)).ToList();

                        //filter the stories, if they don't have URL
                        stories = stories.Where(s => s?.Url != null).ToList();

                        if (!String.IsNullOrEmpty(searchTerm))
                        {
                            if (stories.Count != 0)
                            {
                                stories = SearchNewsBySearchTerm(stories, searchTerm);
                                stories = FilterStoriesByPage(stories, pageNumber, pageSize);
                            }
                        }

                        else
                        {
                            stories = FilterStoriesByPage(stories, pageNumber, pageSize);
                        }
                    }
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }

            return stories;
        }

        private List<HackerNewsStory?> SearchNewsBySearchTerm(List<HackerNewsStory?> stories, string searchTerm)
        {
            stories = stories.Where(s =>
                               s?.Title.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase) > -1)
                               .ToList();

            return stories;
        }

        private List<HackerNewsStory?> FilterStoriesByPage(List<HackerNewsStory?> stories, int pageNumber, int pageSize)
        {
            var skipAmount = (pageNumber - 1) * pageSize;
            stories = stories.Skip(skipAmount).Take(pageSize).ToList();

            return stories;
        }

        private async Task<HackerNewsStory?> GetStoryByIdAsync(int storyId)
        {
            return await _cache.GetOrAddAsync<HackerNewsStory?>(storyId.ToString(),
            async cacheEntry =>
            {
                HackerNewsStory? story = new HackerNewsStory();

                try
                {
                    var response = await _hackerNewsService.GetStoryByIdAsync(storyId);
                    if (response.IsSuccessStatusCode)
                    {
                        var storyResponse = response.Content.ReadAsStringAsync().Result;
                        story = JsonConvert.DeserializeObject<HackerNewsStory>(storyResponse);
                    }
                }
                catch (Exception ex) {
                   throw new Exception(ex.Message);
                }
               return story;
            }, _cacheSettings);
        }
    }
}
