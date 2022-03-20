using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Teams.GetTeam;

public class GetTeamCommandHandler : IRequestHandler<GetTeamCommand, Result<FetchTeam>>
{
    private readonly IManageRepository _manageRepository;

    public GetTeamCommandHandler(IManageRepository manageRepository)
    {
        _manageRepository = manageRepository;
    }

    public async Task<Result<FetchTeam>> Handle(GetTeamCommand request, CancellationToken cancellationToken)
    {
        var rst = await _manageRepository.GetTeam(request.Id, cancellationToken);

        return rst.Id.ToGuid() == Guid.Empty
            ? Result.Fail<FetchTeam>($"Team with id [{request.Id}] does not exist!")
            : Result.Ok(rst);
    }
}
