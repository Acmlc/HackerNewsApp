using AutoFixture;
using HackerNewsApp.Models;
using HackerNewsApp.Queries;
using HackerNewsApp.Services;
using LazyCache.Mocks;
using LazyCache;
using MediatR;
using Moq;
using System.Net;

namespace HackerNewsApp.Tests
{  
    public class GetHackerNewsByNameHandler_Should_
    {
        private IAppCache _cache;
        string content;

        public GetHackerNewsByNameHandler_Should_() {
            _cache = new MockCachingService();
            content = "{\r\n \"title\": \"abc\",\r\n \"by\": \"abc\",\r\n \"url\": \"https://abc/abc/\"\r\n  }";

        }
        [Fact]
        public async Task Return_NewestStories_When_SearchTerm_Is_Not_Null_Or_Empty()
        {
            QueryParams queryParams = new QueryParams()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = "abc"
            };

            // Arrange
            Mock<IHackerNewsService> mockHackerNewsService = new Mock<IHackerNewsService>();
            
            mockHackerNewsService.Setup(
                s => s.NewStoriesAsync())
                .Returns(Task.FromResult<HttpResponseMessage>(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("[1]") }));

            mockHackerNewsService.Setup(
                s => s.GetStoryByIdAsync(1))
                .Returns(Task.FromResult<HttpResponseMessage>
                (new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(content) }));

            GetHackerNewsByNameHandler _handler = new GetHackerNewsByNameHandler(_cache, mockHackerNewsService.Object);
          
            var response = await _handler.Handle(new GetHackerNewsByName(queryParams), CancellationToken.None);
            
            Assert.NotNull(response);
            Assert.Single(response);
        }

        [Fact]
        public async Task Return_NewestStories_When_SearchTerm_Is_Null()
        {
            QueryParams queryParams = new QueryParams()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = null
            };

            // Arrange
            Mock<IHackerNewsService> mockHackerNewsService = new Mock<IHackerNewsService>();

            mockHackerNewsService.Setup(
                s => s.NewStoriesAsync())
                .Returns(Task.FromResult<HttpResponseMessage>(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("[1]") }));

            mockHackerNewsService.Setup(
                s => s.GetStoryByIdAsync(1))
                .Returns(Task.FromResult<HttpResponseMessage>
                (new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(content) }));

            GetHackerNewsByNameHandler _handler = new GetHackerNewsByNameHandler(_cache, mockHackerNewsService.Object);

            var response = await _handler.Handle(new GetHackerNewsByName(queryParams), CancellationToken.None);

            Assert.NotNull(response);
            Assert.Single(response);
        }

        [Fact]
        public async Task Not_Return_Stories_When_StoryId_Not_Available()
        {
            QueryParams queryParams = new QueryParams()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = null
            };

            // Arrange
            Mock<IHackerNewsService> mockHackerNewsService = new Mock<IHackerNewsService>();

            mockHackerNewsService.Setup(
                s => s.NewStoriesAsync())
                .Returns(Task.FromResult<HttpResponseMessage>(new HttpResponseMessage { StatusCode = HttpStatusCode.OK }));

            mockHackerNewsService.Setup(
                s => s.GetStoryByIdAsync(1))
                .Returns(Task.FromResult<HttpResponseMessage>
                (new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(content) }));

            GetHackerNewsByNameHandler _handler = new GetHackerNewsByNameHandler(_cache, mockHackerNewsService.Object);

            var response = await _handler.Handle(new GetHackerNewsByName(queryParams), CancellationToken.None);

            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public async Task Not_Return_NewestStories_When_NewStoriesAsync_Throw_Exception()
        {
            QueryParams queryParams = new QueryParams()
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = "abc"
            };

            // Arrange
            Mock<IHackerNewsService> mockHackerNewsService = new Mock<IHackerNewsService>();

            mockHackerNewsService.Setup(
                s => s.NewStoriesAsync()).Throws(new Exception());

            mockHackerNewsService.Setup(
                s => s.GetStoryByIdAsync(1))
                .Returns(Task.FromResult<HttpResponseMessage>
                (new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(content) }));

            GetHackerNewsByNameHandler _handler = new GetHackerNewsByNameHandler(_cache, mockHackerNewsService.Object);

            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(new GetHackerNewsByName(queryParams), CancellationToken.None));
        }
    }
    
}