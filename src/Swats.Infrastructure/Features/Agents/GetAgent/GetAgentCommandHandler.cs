using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Agents.GetAgent
{
    public class GetAgentCommandHandler : IRequestHandler<GetAgentCommand, Result<FetchedAgent>>
    {
        private readonly IAgentRepository _agentRepository;

        public GetAgentCommandHandler(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        public async Task<Result<FetchedAgent>> Handle(GetAgentCommand request, CancellationToken cancellationToken)
        {
            var result = await _agentRepository.GetAgent(request.Id, cancellationToken);

            return result.Id.ToGuid() == Guid.Empty
                ? Result.Fail<FetchedAgent>($"Agent with id [{request.Id}] does not exist!")
                : Result.Ok(result);
        }
    }
}
