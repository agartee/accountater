using Accountater.Domain.Models;
using Accountater.Domain.Services;
using MediatR;

namespace Accountater.Domain.Commands
{
    public class DeleteCsvImportSchema : IRequest
    {
        public required CsvImportSchemaId Id { get; init; }
    }

    public class DeleteCsvImportSchemaHandler : IRequestHandler<DeleteCsvImportSchema>
    {
        private readonly ICsvImportSchemaRepository csvImportSchemaRepository;

        public DeleteCsvImportSchemaHandler(ICsvImportSchemaRepository csvImportSchemaRepository)
        {
            this.csvImportSchemaRepository = csvImportSchemaRepository;
        }

        public async Task Handle(DeleteCsvImportSchema request, CancellationToken cancellationToken)
        {
            await csvImportSchemaRepository.DeleteCsvImportSchema(request.Id, cancellationToken);
        }
    }
}
