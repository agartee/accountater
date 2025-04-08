using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record DemandFinancialTransaction : IRequest<FinancialTransactionInfo>
    {
        public required FinancialTransactionId Id { get; init; }
    }

    public class GetFinancialTransactionHandler : IRequestHandler<DemandFinancialTransaction, FinancialTransactionInfo>
    {
        private readonly IFinancialTransactionRepository financialTransactionRepository;

        public GetFinancialTransactionHandler(IFinancialTransactionRepository financialTransactionRepository)
        {
            this.financialTransactionRepository = financialTransactionRepository;
        }

        public Task<FinancialTransactionInfo> Handle(DemandFinancialTransaction request, CancellationToken cancellationToken)
        {
            return financialTransactionRepository.DemandFinancialTransactionInfo(request.Id, cancellationToken);
        }
    }
}
