using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record ImportCreditTransactions : IRequest
    {
        public required IEnumerable<CreditTransaction> Transactions { get; set; }
    }

    public class ImportCreditTransactionsHandler : IRequestHandler<ImportCreditTransactions>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;

        public ImportCreditTransactionsHandler(IFinancialTransactionRepository financialTransactionRepository)
        {
            this.financialTransactionRepository = financialTransactionRepository;
        }

        public async Task Handle(ImportCreditTransactions request, CancellationToken cancellationToken)
        {
            await financialTransactionRepository.InsertCreditTransactions(request.Transactions);
        }
    }
}
