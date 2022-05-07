using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Agents.GetAgent;

public class GetAgentCommand : GetType, IRequest<Result<FetchAgent>>
{
}