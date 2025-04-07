using Accountater.Domain.Models;

namespace Accountater.Domain.Services
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagInfo>> GetTagInfos(CancellationToken cancellationToken);
        Task<TagInfo> SaveTag(Tag tag, CancellationToken cancellationToken);
        Task<Tag> DemandTag(TagId id, CancellationToken cancellationToken);
        Task<TagInfo> DemandTagInfo(TagId id, CancellationToken cancellationToken);
        Task DeleteTag(TagId id, CancellationToken cancellationToken);
        Task<TagSearchResults> SearchTags(SearchCriteria criteria, CancellationToken cancellationToken);
    }
}
