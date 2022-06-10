using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Template.ListTemplates
{

    internal class ListTemplatesCommandHandler : IRequestHandler<ListTemplatesCommand, Result<IEnumerable<FetchTemplate>>>
    {
        private readonly IManageRepository _manageRepository;

        public ListTemplatesCommandHandler(IManageRepository manageRepository)
        {
            _manageRepository = manageRepository;
        }

        public async Task<Result<IEnumerable<FetchTemplate>>> Handle(ListTemplatesCommand request, CancellationToken cancellationToken)
        {
            var rst = await _manageRepository.ListTemplates(request.Offset, request.Limit, request.Deleted, cancellationToken);

            return Result.Ok(rst);
        }
    }
}
