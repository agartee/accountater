using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record UpdateFinancialTransaction : IRequest
    {
        public required FinancialTransactionId Id { get; init; }
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
            await financialTransactionRepository.UpdateFinancialTransactionTags(
                request.Id, request.Tags, cancellationToken);
        }
    }
}
