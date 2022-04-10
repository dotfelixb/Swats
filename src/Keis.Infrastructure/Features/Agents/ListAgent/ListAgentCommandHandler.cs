using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;

namespace Keis.Infrastructure.Features.Agents.ListAgent;

public class ListAgentCommandHandler : IRequestHandler<ListAgentCommand, Result<IEnumerable<FetchAgent>>>
{
    private readonly IAgentRepository _agentRepository;

    public ListAgentCommandHandler(IAgentRepository agentRepository)
    {
        _agentRepository = agentRepository;
    }

    public async Task<Result<IEnumerable<FetchAgent>>> Handle(ListAgentCommand request,
        CancellationToken cancellationToken)
    {
        var result =
            await _agentRepository.ListAgent(request.Offset, request.Limit, request.Deleted, cancellationToken);
        return Result.Ok(result);
    }
}