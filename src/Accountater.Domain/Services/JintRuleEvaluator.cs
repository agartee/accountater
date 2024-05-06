using Accountater.Domain.Models;
using Jint;

namespace Accountater.Domain.Services
{
    public interface IRuleEvaluator
    {
        bool Evaluate(string expression, FinancialTransactionInfo financialTransaction);
    }

    public class JintRuleEvaluator : IRuleEvaluator
    {
        public bool Evaluate(string expression, FinancialTransactionInfo financialTransaction)
        {
            var engine = new Engine()
                .SetValue("transaction", financialTransaction);

            return engine.Evaluate(expression).AsBoolean();
        }
    }
}
