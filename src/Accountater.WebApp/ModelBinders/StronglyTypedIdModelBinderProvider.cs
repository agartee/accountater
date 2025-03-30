using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Reflection;

namespace Accountater.WebApp.ModelBinders
{
    public class StronglyTypedIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;

            if (!modelType.IsValueType || modelType.IsPrimitive || modelType.IsEnum)
                return null;

            // ✅ Use explicit binding flags to ensure we get the primary constructor property
            var props = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var hasSingleValueProperty = props.Length == 1 &&
                (props[0].PropertyType == typeof(Guid) || props[0].PropertyType == typeof(string));

            if (!hasSingleValueProperty)
                return null;

            return new BinderTypeModelBinder(typeof(StronglyTypedIdModelBinder));
        }
    }
}
