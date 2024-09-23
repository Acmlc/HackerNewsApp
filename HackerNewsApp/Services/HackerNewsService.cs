namespace HackerNewsApp.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private HttpClient _client = new HttpClient();
        private readonly ILogger<HackerNewsService> _logger;
        public HackerNewsService(HttpClient client, ILogger<HackerNewsService> logger)
        {
            _client = client;
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
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
            }
        }
    }
}
