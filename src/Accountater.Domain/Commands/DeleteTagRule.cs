using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public record DeleteTagRule : IRequest
    {
        public required TagRuleId Id { get; init; }
    }

    public class DeleteTagRuleHandler : IRequestHandler<DeleteTagRule>
    {
        private readonly ITagRuleRepository tagRuleRepository;

        public DeleteTagRuleHandler(ITagRuleRepository tagRuleRepository)
        {
            this.tagRuleRepository = tagRuleRepository;
        }

        public async Task Handle(DeleteTagRule request, CancellationToken cancellationToken)
        {
            await tagRuleRepository.DeleteTagRule(request.Id, cancellationToken);
        }
    }
}
