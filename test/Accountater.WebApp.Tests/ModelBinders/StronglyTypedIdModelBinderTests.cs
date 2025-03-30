using Accountater.WebApp.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Primitives;

namespace Accountater.WebApp.Tests.ModelBinders
{
    public class StronglyTypedIdModelBinderTests
    {
        private static ModelBindingContext CreateContext<T>(string modelName, string? rawValue)
        {
            var valueProvider = new Mock<IValueProvider>();
            valueProvider
                .Setup(vp => vp.GetValue(modelName))
                .Returns(new ValueProviderResult(new StringValues(rawValue)));

            var metadataProvider = new EmptyModelMetadataProvider();
            var modelMetadata = metadataProvider.GetMetadataForType(typeof(T));

            return new DefaultModelBindingContext
            {
                ModelMetadata = modelMetadata,
                ModelName = modelName,
                ValueProvider = valueProvider.Object,
                ModelState = new ModelStateDictionary()
            };
        }

        [Fact]
        public async Task Binds_Guid_Id_Successfully()
        {
            var guid = Guid.NewGuid();
            var context = CreateContext<OrderId>("id", guid.ToString());

            var binder = new StronglyTypedIdModelBinder();
            await binder.BindModelAsync(context);

            context.Result.IsModelSet.Should().BeTrue();
            context.Result.Model.Should().Be(new OrderId(guid));
        }

        [Fact]
        public async Task Fails_Invalid_Guid()
        {
            var context = CreateContext<OrderId>("id", "not-a-guid");

            var binder = new StronglyTypedIdModelBinder();
            await binder.BindModelAsync(context);

            context.Result.IsModelSet.Should().BeFalse();
            context.ModelState["id"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Be("Invalid GUID format.");
        }

        [Fact]
        public async Task Binds_String_Id_Successfully()
        {
            var context = CreateContext<ProductSku>("sku", "abc-123");

            var binder = new StronglyTypedIdModelBinder();
            await binder.BindModelAsync(context);

            context.Result.IsModelSet.Should().BeTrue();
            context.Result.Model.Should().Be(new ProductSku("abc-123"));
        }

        [Fact]
        public async Task Fails_When_No_Constructor_Found()
        {
            var context = CreateContext<NoCtorId>("id", "123");

            var binder = new StronglyTypedIdModelBinder();
            await binder.BindModelAsync(context);

            context.Result.IsModelSet.Should().BeFalse();
            context.ModelState["id"]!.Errors.Should().ContainSingle()
                .Which.ErrorMessage.Should().Contain("No suitable constructor");
        }

        public readonly record struct OrderId(Guid Value);
        public readonly record struct ProductSku(string Value);
        public readonly record struct NoCtorId(int Value);
    }
}
