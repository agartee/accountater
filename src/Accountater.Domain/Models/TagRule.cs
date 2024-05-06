namespace Accountater.Domain.Models
{
    public record TagRule
    {
        public required TagRuleId Id { get; init; }
        public required string Name { get; set; }
        public required string Expression { get; set; }
        public required string Tag { get; set; }
    }
}
