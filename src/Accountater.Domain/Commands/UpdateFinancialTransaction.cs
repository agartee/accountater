using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateFinancialTransaction : IRequest
    {
        public required FinancialTransactionId Id { get; init; }
        public CategoryId? CategoryId { get; init; }
        public IEnumerable<string> Tags { get; init; } = new List<string>().AsReadOnly();
    }

    public class UpdateFinancialTransactionHandler : IRequestHandler<UpdateFinancialTransaction>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;

        public UpdateFinancialTransactionHandler(IFinancialTransactionRepository financialTransactionRepository)
        {
            this.financialTransactionRepository = financialTransactionRepository;
        }

        public async Task Handle(UpdateFinancialTransaction request, CancellationToken cancellationToken)
        {
            var transaction = await financialTransactionRepository.DemandFinancialTransaction(request.Id, cancellationToken);

            transaction.CategoryId = request.CategoryId;

            transaction.Tags.Clear();
            transaction.Tags.AddRange(request.Tags);

            await financialTransactionRepository.UpdateFinancialTransaction(transaction, cancellationToken);
        }
    }
}
