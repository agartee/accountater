using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Accountater.WebApp.ModelBinders
{
    public class StronglyTypedIdModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            if (!Guid.TryParse(value, out var guidValue))
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid GUID format.");
                return Task.CompletedTask;
            }

            var modelType = bindingContext.ModelType;
            var ctor = modelType.GetConstructor(new[] { typeof(Guid) });

            if (ctor == null)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, $"No suitable constructor found on {modelType.Name}.");
                return Task.CompletedTask;
            }

            var model = ctor.Invoke(new object[] { guidValue });
            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
