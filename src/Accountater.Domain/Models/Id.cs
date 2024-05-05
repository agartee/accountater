namespace Accountater.Domain.Models
{
    public record Id<T>
    {
        public Id(T value)
        {
            Value = value;
        }

        public T Value { get; init; }
    }
}
