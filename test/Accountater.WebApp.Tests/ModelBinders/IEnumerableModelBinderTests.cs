using Accountater.WebApp.ModelBinders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Accountater.WebApp.Tests.ModelBinders
{
    // todo: refactor tests (naming and assertions)
    public class IEnumerableModelBinderTests
    {
        // Simple test-only model
        private record TestItem(string Name, int Value);

        [Fact]
        public async Task Binds_Contiguous_Indices_Correctly()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["Items[0].Name"] = "Item 0",
                ["Items[0].Value"] = "10",
                ["Items[1].Name"] = "Item 1",
                ["Items[1].Value"] = "20"
            };

            var binder = CreateBinder<TestItem>(formData, "Items", out var context);

            // Act
            await binder.BindModelAsync(context);
            var result = context.Result.Model as List<TestItem>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Item 0", result[0].Name);
            Assert.Equal(20, result[1].Value);
        }

        [Fact]
        public async Task Binds_Sparse_Indices_Correctly()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["Items[0].Name"] = "Item 0",
                ["Items[0].Value"] = "10",
                ["Items[2].Name"] = "Item 2",
                ["Items[2].Value"] = "30"
            };

            var binder = CreateBinder<TestItem>(formData, "Items", out var context);

            // Act
            await binder.BindModelAsync(context);
            var result = context.Result.Model as List<TestItem>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Item 0", result[0].Name);
            Assert.Equal(30, result[1].Value); // Index 2 becomes logical second item
        }

        [Fact]
        public async Task Skips_Missing_Items_Gracefully()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["Items[0].Name"] = "Item 0",
                ["Items[0].Value"] = "10",
                ["Items[3].Name"] = "Item 3",
                ["Items[3].Value"] = "40"
            };

            var binder = CreateBinder<TestItem>(formData, "Items", out var context);

            // Act
            await binder.BindModelAsync(context);
            var result = context.Result.Model as List<TestItem>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Item 0", result[0].Name);
            Assert.Equal("Item 3", result[1].Name);
        }

        private static IEnumerableModelBinder<T> CreateBinder<T>(
            Dictionary<string, string> formData,
            string modelName,
            out DefaultModelBindingContext context)
        {
            var formCollection = new FormCollection(formData.ToDictionary(kv => kv.Key, kv => new Microsoft.Extensions.Primitives.StringValues(kv.Value)));

            var valueProvider = new FormValueProvider(
                BindingSource.Form,
                formCollection,
                System.Globalization.CultureInfo.InvariantCulture
            );

            var metadataProvider = new EmptyModelMetadataProvider();
            var modelMetadata = metadataProvider.GetMetadataForType(typeof(List<T>));
            var elementMetadata = metadataProvider.GetMetadataForType(typeof(T));

            var binder = new StubModelBinder<T>();

            var modelBinder = new IEnumerableModelBinder<T>(binder);

            context = new DefaultModelBindingContext
            {
                ModelMetadata = modelMetadata,
                ModelName = modelName,
                ModelState = new ModelStateDictionary(),
                ValueProvider = valueProvider,
                ActionContext = new ActionContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = new ServiceCollection().BuildServiceProvider()
                    }
                }
            };

            return modelBinder;
        }

        private class StubModelBinder<TElement> : IModelBinder
        {
            public Task BindModelAsync(ModelBindingContext bindingContext)
            {
                var namePrefix = bindingContext.ModelName;

                var nameResult = bindingContext.ValueProvider.GetValue($"{namePrefix}.Name");
                var valueResult = bindingContext.ValueProvider.GetValue($"{namePrefix}.Value");

                if (nameResult == ValueProviderResult.None || valueResult == ValueProviderResult.None)
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return Task.CompletedTask;
                }

                var name = nameResult.FirstValue!;
                var value = int.Parse(valueResult.FirstValue!);

                object instance = new TestItem(name, value);
                bindingContext.Result = ModelBindingResult.Success(instance);

                return Task.CompletedTask;
            }
        }

    }
}
