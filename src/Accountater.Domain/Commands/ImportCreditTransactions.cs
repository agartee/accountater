using Accountater.Domain.Models;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record ImportCreditTransactions : IRequest
    {
        public required IEnumerable<CreditTransaction> Transactions { get; set; }
    }

    public class ImportCreditTransactionsHandler : IRequestHandler<ImportCreditTransactions>
    {
        public async Task Handle(ImportCreditTransactions request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
