using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Agents.ListAgent;

public class ListAgentCommand : ListType, IRequest<Result<IEnumerable<FetchAgent>>>
{
}