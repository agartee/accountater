using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Accountater.WebApp.ModelBinders
{
    public class StronglyTypedIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;

            // Unwrap nullable types
            var underlyingType = Nullable.GetUnderlyingType(modelType) ?? modelType;

            // Check for strongly-typed ID pattern: value type with a Guid or string constructor
            if ((underlyingType.IsValueType || underlyingType.IsClass) &&
                (underlyingType.GetConstructor(new[] { typeof(Guid) }) != null ||
                 underlyingType.GetConstructor(new[] { typeof(string) }) != null))
            {
                return new BinderTypeModelBinder(typeof(StronglyTypedIdModelBinder));
            }

            return null;
        }
    }
}
