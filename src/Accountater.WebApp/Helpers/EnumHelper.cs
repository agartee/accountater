using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Reflection;

namespace Accountater.WebApp.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<SelectListItem> CreateSelectList<TEnum>(TEnum? selected = null) where TEnum : struct, Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.GetDescription(),
                    Value = Convert.ToInt32(e).ToString(),
                    Selected = selected.HasValue && e.Equals(selected.Value)
                });
        }

        public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
        {
            var field = typeof(TEnum).GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }
    }
}
