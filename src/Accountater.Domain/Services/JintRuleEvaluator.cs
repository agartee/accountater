using Accountater.Domain.Models;
using Jint;

namespace Accountater.Domain.Services
{
    public interface IFinancialTransactionRuleEvaluator
    {
        bool Evaluate(string expression, FinancialTransactionInfo financialTransaction);
    }

    public class JintRuleEvaluator : IFinancialTransactionRuleEvaluator
    {
        public bool Evaluate(string expression, FinancialTransactionInfo financialTransaction)
        {
            var engine = new Engine()
                .SetValue("transaction", financialTransaction);

            return engine.Evaluate(expression).AsBoolean();
        }
    }
}
