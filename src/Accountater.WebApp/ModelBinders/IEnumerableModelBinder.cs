using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Accountater.WebApp.ModelBinders
{
    public class IEnumerableModelBinder<T> : IModelBinder
    {
        private readonly IModelBinder _elementBinder;

        public IEnumerableModelBinder(IModelBinder elementBinder)
        {
            _elementBinder = elementBinder;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var result = new List<T>();
            int index = 0;

            while (true)
            {
                var modelName = $"{bindingContext.ModelName}[{index}]";

                if (!bindingContext.ValueProvider.ContainsPrefix(modelName))
                    break;

                var elementMetadata = bindingContext.ModelMetadata.ElementMetadata!;
                var childContext = DefaultModelBindingContext.CreateBindingContext(
                    bindingContext.ActionContext,
                    bindingContext.ValueProvider,
                    elementMetadata,
                    bindingInfo: null,
                    modelName: modelName
                );

                await _elementBinder.BindModelAsync(childContext);

                if (childContext.Result.IsModelSet)
                {
                    result.Add((T)childContext.Result.Model!);
                }

                index++;
            }

            bindingContext.Result = ModelBindingResult.Success(result);
        }
    }
}
