using Accountater.Domain.Models;
using Accountater.WebApp.Attributes;

namespace Accountater.WebApp.Models
{
    public record UploadCsvFileViewModel
    {
        public required AccountId SelectedAccountId { get; init; }

        [ValidCsvFile]
        public IFormFile? File { get; init; }
    }
}
