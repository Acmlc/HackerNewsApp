using Microsoft.AspNetCore.Mvc.Testing;

public class HackerNewsController
: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HackerNewsController(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("https://localhost:7267/HackerNews?PageNumber=1&PageSize=10&SearchTerm=a")]
    public async Task Return_GetHackerNewsRequest_Successful(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); 
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }
}
