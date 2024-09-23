namespace HackerNewsApp.Services
{
    public interface IHackerNewsService
    {
        Task<HttpResponseMessage> NewStoriesAsync();
        Task<HttpResponseMessage> GetStoryByIdAsync(int id);
    }
}
