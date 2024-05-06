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
            var transaction = new FinancialTransactionInfo
            {
                Id = FinancialTransactionId.NewId(),
                Account = new AccountInfo
                {
                    Id = AccountId.NewId(),
                    Name = "Account 1",
                },
                Amount = 11,
                Description = "description",
                TransactionDate = DateTime.UtcNow,
            };

            var rule = new TagRule
            {
                Id = TagRuleId.NewId(),
                Name = "test rule",
                Expression = "transaction.Amount > 10",
                Tag = "tag"
            };

            var result = ruleEvaluator.Evaluate(rule.Expression, transaction);

            result.Should().BeTrue();
        }
    }
}
