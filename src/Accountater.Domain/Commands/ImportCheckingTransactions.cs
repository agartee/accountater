using Accountater.Domain.Models;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record ImportCheckingTransactions : IRequest
    {
        public required IEnumerable<CheckingTransaction> Transactions { get; set; }
    }

    public class ImportCheckingTransactionsHandler : IRequestHandler<ImportCheckingTransactions>
    {
        public async Task Handle(ImportCheckingTransactions request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
