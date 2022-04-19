using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Teams.GetTeam;

public class GetTeamCommand : GetType, IRequest<Result<FetchTeam>>
{
}