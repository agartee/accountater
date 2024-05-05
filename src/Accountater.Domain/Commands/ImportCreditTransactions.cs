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
        private readonly ICreditTransactionRepository creditTransactionRepository;

        public ImportCreditTransactionsHandler(ICreditTransactionRepository creditTransactionRepository)
        {
            this.creditTransactionRepository = creditTransactionRepository;
        }

        public async Task Handle(ImportCreditTransactions request, CancellationToken cancellationToken)
        {
            await creditTransactionRepository.InsertCreditTransactions(request.Transactions);
        }
    }
}
