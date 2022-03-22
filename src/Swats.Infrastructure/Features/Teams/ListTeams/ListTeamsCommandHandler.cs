using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Teams.ListTeams;

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