using Accountater.Domain.Models;
using Accountater.WebApp.Models;

namespace Accountater.WebApp.Extensions
{
    public static class AccountExtensions
    {
        public static EditAccountViewModel ToAccountViewModel(this AccountInfo model)
        {
            return new EditAccountViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };
        }
    }
}
