using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

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
            var modelName = bindingContext.ModelName;

            // This regex finds indices in names like "Mappings[2].MappedProperty"
            var indexRegex = new Regex($@"^{Regex.Escape(modelName)}\[(\d+)\]", RegexOptions.Compiled);

            // Use the ValueProvider to extract all keys by probing known prefixes
            var indices = new SortedSet<int>();

            // Try a reasonable range of indices (e.g., 0–99)
            for (int i = 0; i < 100; i++)
            {
                var prefix = $"{modelName}[{i}]";
                if (!bindingContext.ValueProvider.ContainsPrefix(prefix))
                    continue;

                indices.Add(i);
            }

            foreach (var index in indices)
            {
                var itemPrefix = $"{modelName}[{index}]";
                var elementMetadata = bindingContext.ModelMetadata.ElementMetadata!;

                var childContext = DefaultModelBindingContext.CreateBindingContext(
                    bindingContext.ActionContext,
                    bindingContext.ValueProvider,
                    elementMetadata,
                    bindingInfo: null,
                    modelName: itemPrefix
                );

                await _elementBinder.BindModelAsync(childContext);

                if (childContext.Result.IsModelSet)
                {
                    result.Add((T)childContext.Result.Model!);
                }
            }

            bindingContext.Result = ModelBindingResult.Success(result);
        }
    }
}
