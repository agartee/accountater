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
        private readonly ICheckingTransactionRepository checkingTransactionRepository;

        public ImportCheckingTransactionsHandler(ICheckingTransactionRepository checkingTransactionRepository)
        {
            this.checkingTransactionRepository = checkingTransactionRepository;
        }

        public async Task Handle(ImportCheckingTransactions request, CancellationToken cancellationToken)
        {
            await checkingTransactionRepository.InsertCheckingTransactions(request.Transactions);
        }
    }
}
