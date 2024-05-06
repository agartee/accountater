namespace Accountater.WebApp.Models
{
    public class CreateTagRuleViewModel
    {
        public required string Name { get; set; }
        public required string Expression { get; set; }
        public required string Tag { get; set; }
    }
}
