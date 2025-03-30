using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Accountater.WebApp.ModelBinders
{
    public class StronglyTypedIdModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            var modelType = context.Metadata.ModelType;

            if (!modelType.IsValueType || modelType.IsPrimitive || modelType.IsEnum)
                return null;

            var ctors = modelType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            var hasSupportedCtor = ctors.Any(c =>
            {
                var parameters = c.GetParameters();
                return parameters.Length == 1 &&
                       (parameters[0].ParameterType == typeof(Guid) ||
                        parameters[0].ParameterType == typeof(string));
            });

            return hasSupportedCtor
                ? new BinderTypeModelBinder(typeof(StronglyTypedIdModelBinder))
                : null;
        }
    }
}
