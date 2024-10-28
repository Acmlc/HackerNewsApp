using HackerNewsApp.Models;
using MediatR;

namespace HackerNewsApp.Queries
{
    public class GetHackerNewsByName : IRequest<IEnumerable<HackerNewsStory>>
    {
        public string? searchTerm { get; }
        public int pageSize { get; }
        public int pageNumber { get; }

        public GetHackerNewsByName(QueryParams parameters)
        {
            searchTerm = parameters.SearchTerm;
            pageSize = parameters.PageSize;
            pageNumber = parameters.PageNumber;
        }
    }
}
