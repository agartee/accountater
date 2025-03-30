using Accountater.WebApp.ModelBinders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentAssertions;
using Moq;

namespace Accountater.WebApp.Tests.ModelBinders
{
    public class StronglyTypedIdModelBinderTests
    {
        [Fact]
        public void Should_return_binder_for_supported_value_type()
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForType(typeof(TestId));
            var context = new Mock<ModelBinderProviderContext>();
            context.Setup(c => c.Metadata).Returns(metadata);
            var provider = new StronglyTypedIdModelBinderProvider();

            var binder = provider.GetBinder(context.Object);

            binder.Should().NotBeNull();
            binder.Should().BeOfType<BinderTypeModelBinder>();
        }

        [Fact]
        public void Should_return_null_for_unsupported_type()
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForType(typeof(string));
            var context = new Mock<ModelBinderProviderContext>();
            context.Setup(c => c.Metadata).Returns(metadata);
            var provider = new StronglyTypedIdModelBinderProvider();

            var binder = provider.GetBinder(context.Object);

            binder.Should().BeNull();
        }

        public readonly record struct TestId(Guid Value)
        {
            public static TestId New() => new(Guid.NewGuid());
        }
    }
}
