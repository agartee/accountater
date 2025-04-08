using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Accountater.WebApp.ModelBinders
{
    public class StronglyTypedIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
            var rawValue = valueProviderResult.FirstValue;

            if (string.IsNullOrWhiteSpace(rawValue))
            {
                // If it's a nullable type, just return null
                if (IsNullable(bindingContext.ModelType))
                {
                    bindingContext.Result = ModelBindingResult.Success(null);
                }
                return Task.CompletedTask;
            }

            var modelType = Nullable.GetUnderlyingType(bindingContext.ModelType) ?? bindingContext.ModelType;

            var ctor = modelType.GetConstructor(new[] { typeof(Guid) }) ??
                       modelType.GetConstructor(new[] { typeof(string) });

            if (ctor == null)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"No suitable constructor found on {modelType.Name}.");
                return Task.CompletedTask;
            }

            object? convertedValue = null;

            if (ctor.GetParameters()[0].ParameterType == typeof(Guid))
            {
                if (!Guid.TryParse(rawValue, out var guid))
                {
                    bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid GUID format.");
                    return Task.CompletedTask;
                }
                convertedValue = guid;
            }
            else if (ctor.GetParameters()[0].ParameterType == typeof(string))
            {
                convertedValue = rawValue;
            }

            var model = ctor.Invoke(new[] { convertedValue! });
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }

        private static bool IsNullable(Type type) =>
            Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
    }

}
