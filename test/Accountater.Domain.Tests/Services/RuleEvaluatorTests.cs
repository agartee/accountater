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
                    Type = AccountType.Bank,
                    Description = "test account description"
                },
                Amount = 11,
                Description = "description",
                Date = DateTime.UtcNow,
                Tags = [ "VERIZON" ]
            };

            var rule = new FinancialTransactionMetadataRule
            {
                Id = FinancialTransactionMetadataRuleId.NewId(),
                Name = "test rule",
                Expression = "financialTransaction.tags.includes('VERIZON')",
                MetadataType = FinancialTransactionMetadataType.Tag,
                MetadataValue = "test"
            };

            var result = ruleEvaluator.Evaluate(rule.Expression, financialTransaction);

            result.Should().BeTrue();
        }
    }
}
