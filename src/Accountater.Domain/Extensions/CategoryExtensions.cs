using Accountater.Domain.Models;

namespace Accountater.Domain.Extensions
{
    public static class CategoryExtensions
    {
        public static CategoryInfo ToCategoryInfo(this Category category)
        {
            return new CategoryInfo
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
