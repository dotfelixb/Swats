using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Teams.ListTeams;

public class ListTeamsCommand : ListType, IRequest<Result<IEnumerable<FetchTeam>>>
{
}