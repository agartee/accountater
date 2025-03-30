using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Accountater.WebApp.ModelBinders
{
    public class StronglyTypedIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;

            // Must be a user-defined struct (not primitive or enum)
            if (!modelType.IsValueType || modelType.IsPrimitive || modelType.IsEnum)
                return null;

            // Check for exactly one public readable property of type Guid or string
            var property = modelType
                .GetProperties()
                .Where(p => p.CanRead &&
                            (p.PropertyType == typeof(Guid) || p.PropertyType == typeof(string)))
                .SingleOrDefault();

            if (property == null)
                return null;

            return new BinderTypeModelBinder(typeof(StronglyTypedIdModelBinder));
        }
    }
}
