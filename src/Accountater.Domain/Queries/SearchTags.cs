using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record SearchTags : IRequest<TagSearchResults>
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }

    public class SearchTagsHandler
        : IRequestHandler<SearchTags, TagSearchResults>
    {
        private readonly ITagRepository tagRepository;

        public SearchTagsHandler(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        public async Task<TagSearchResults> Handle(
            SearchTags request, CancellationToken cancellationToken)
        {
            return await tagRepository.SearchTags(
                new SearchCriteria
                {
                    SearchText = request.SearchText,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }, cancellationToken);
        }
    }
}
