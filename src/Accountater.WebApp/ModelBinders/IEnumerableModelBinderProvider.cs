using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Accountater.WebApp.ModelBinders
{
    public class IEnumerableModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (!context.Metadata.IsEnumerableType || context.Metadata.ModelType == typeof(string))
                return null;

            var elementType = context.Metadata.ElementType!;
            var elementMetadata = context.MetadataProvider.GetMetadataForType(elementType);
            var elementBinder = context.CreateBinder(elementMetadata);

            var binderType = typeof(IEnumerableModelBinder<>).MakeGenericType(elementType);
            return (IModelBinder)Activator.CreateInstance(binderType, elementBinder)!;
        }
    }
}
