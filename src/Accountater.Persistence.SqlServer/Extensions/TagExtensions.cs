using Accountater.Domain.Models;
using Accountater.Persistence.SqlServer.Models;

namespace Accountater.Persistence.SqlServer.Extensions
{
    public static class TagExtensions
    {
        public static TagInfo ToTagInfo(this TagData model)
        {
            return new TagInfo
            {
                Id = new TagId(model.Id),
                Value = model.Value,
                Color = model.Color,
                Order = model.Order
            };
        }

        public static Tag ToTag(this TagData model)
        {
            return new Tag
            {
                Id = new TagId(model.Id),
                Value = model.Value,
                Color = model.Color,
                Order = model.Order
            };
        }
    }
}
