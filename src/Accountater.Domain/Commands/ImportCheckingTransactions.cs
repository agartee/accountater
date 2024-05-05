using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record ImportCheckingTransactions : IRequest
    {
        public required IEnumerable<CheckingTransaction> Transactions { get; set; }
    }

    public class ImportCheckingTransactionsHandler : IRequestHandler<ImportCheckingTransactions>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;

        public ImportCheckingTransactionsHandler(IFinancialTransactionRepository financialTransactionRepository)
        {
            this.financialTransactionRepository = financialTransactionRepository;
        }

        public async Task Handle(ImportCheckingTransactions request, CancellationToken cancellationToken)
        {
            await financialTransactionRepository.InsertCheckingTransactions(request.Transactions, cancellationToken);
        }
    }
}
