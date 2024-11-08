﻿namespace HackerNewsApp.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _client;
        private readonly ILogger<HackerNewsService> _logger;
        public HackerNewsService(ILogger<HackerNewsService> logger)
        {
            _client =  new HttpClient();
            _logger = logger;
        }

        public async Task<HttpResponseMessage> NewStoriesAsync()
        {
            try {
                return await _client.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<HttpResponseMessage> GetStoryByIdAsync(int id)
        {
            try
            {
                return await _client.GetAsync(string.Format("https://hacker-news.firebaseio.com/v0/item/{0}.json", id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
