using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Teams.ListTeams;

public class ListTeamsCommandHandler : IRequestHandler<ListTeamsCommand, Result<IEnumerable<FetchTeam>>>
{
    private readonly IManageRepository _manageRepository;

    public ListTeamsCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<IEnumerable<FetchTeam>>> Handle(ListTeamsCommand request,
        CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.ListTeams(request.Offset, request.Limit, request.Deleted, cancellationToken);

        return Result.Ok(rst);
    }
}