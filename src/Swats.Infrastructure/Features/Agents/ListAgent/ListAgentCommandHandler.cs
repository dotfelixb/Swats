using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Agents.ListAgent;

public class ListAgentCommandHandler : IRequestHandler<ListAgentCommand, Result<IEnumerable<FetchedAgent>>>
{
    private readonly IAgentRepository _agentRepository;

    public ListAgentCommandHandler(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public async Task<Result<IEnumerable<FetchedAgent>>> Handle(ListAgentCommand request, CancellationToken cancellationToken)
    {
        var result = await _agentRepository.ListAgent(request.Offset, request.Limit, request.Deleted, cancellationToken);
        return Result.Ok(result);
    }
}
