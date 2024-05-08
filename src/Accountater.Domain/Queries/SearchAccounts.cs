using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Queries
{
    public record SearchAccounts : IRequest<AccountSearchResults>
    {
        public string? SearchText { get; init; }
        public int PageSize { get; init; } = 50;
        public int PageIndex { get; init; } = 0;
    }

    public class SearchAccountsHandler
        : IRequestHandler<SearchAccounts, AccountSearchResults>
    {
        private readonly IAccountRepository accountRepository;

        public SearchAccountsHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<AccountSearchResults> Handle(
            SearchAccounts request, CancellationToken cancellationToken)
        {
            return await accountRepository.SearchAccounts(
                new SearchCriteria
                {
                    SearchText = request.SearchText,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                }, cancellationToken);
        }
    }
}
