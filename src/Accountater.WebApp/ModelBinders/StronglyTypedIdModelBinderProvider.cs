using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Accountater.WebApp.ModelBinders
{
    public class StronglyTypedIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;

            if (modelType.IsValueType && 
               (modelType.GetConstructor([typeof(Guid)]) != null || modelType.GetConstructor([typeof(string)]) != null))
            {
                return new BinderTypeModelBinder(typeof(StronglyTypedIdModelBinder));
            }

            return null;
        }
    }
}
