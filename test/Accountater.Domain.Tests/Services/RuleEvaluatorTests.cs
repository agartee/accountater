using Accountater.Domain.Models;
using Accountater.Domain.Services;
using FluentAssertions;

namespace Accountater.Domain.Tests.Services
{
    public class RuleEvaluatorTests
    {
        private readonly JintRuleEvaluator ruleEvaluator = new JintRuleEvaluator();

        [Fact]
        public void SimpleTest()
        {
            var financialTransaction = new FinancialTransactionInfo
            {
                Id = FinancialTransactionId.NewId(),
                Account = new AccountInfo
                {
                    Id = AccountId.NewId(),
                    Name = "test account",
                    Description = "test account description"
                },
                Amount = 11,
                Description = "description",
                Date = DateTime.UtcNow,
                Tags = [ "VERIZON" ]
            };

            var rule = new TagRule
            {
                Id = TagRuleId.NewId(),
                Name = "test rule",
                Expression = "financialTransaction.tags.includes('VERIZON')",
                Tag = "tag"
            };

            var result = ruleEvaluator.Evaluate(rule.Expression, financialTransaction);

            result.Should().BeTrue();
        }
    }
}
