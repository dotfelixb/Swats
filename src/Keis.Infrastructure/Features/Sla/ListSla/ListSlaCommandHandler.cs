using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Sla.ListSla;

public class ListSlaCommandHandler : IRequestHandler<ListSlaCommand, Result<IEnumerable<FetchSla>>>
{
    private readonly IManageRepository _manageRepository;

    public ListSlaCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchSla>>> Handle(ListSlaCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListSla(request.Offset, request.Limit, request.Deleted, cancellationToken);

        return Result.Ok(rst);
    }
}
