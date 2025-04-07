using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;

namespace Accountater.Persistence.SqlServer.Extensions
{
    public static class CategoryExtnesions
    {
        public static CategoryInfo ToCategoryInfo(this CategoryData model)
        {
            return new CategoryInfo
            {
                Id = new CategoryId(model.Id),
                Name = model.Name
            };
        }

        public static Category ToCategory(this CategoryData model)
        {
            return new Category
            {
                Id = new CategoryId(model.Id),
                Name = model.Name
            };
        }
    }
    }
