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
        private readonly IFinancialTransactionRepository fincialTransactionRepository;

        public ImportCheckingTransactionsHandler(IFinancialTransactionRepository fincialTransactionRepository)
        {
            this.fincialTransactionRepository = fincialTransactionRepository;
        }

        public async Task Handle(ImportCheckingTransactions request, CancellationToken cancellationToken)
        {
            await fincialTransactionRepository.InsertCheckingTransactions(request.Transactions);
        }
    }
}
