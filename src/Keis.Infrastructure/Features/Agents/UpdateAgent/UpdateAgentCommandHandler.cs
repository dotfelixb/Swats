using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Agents.UpdateAgent
{
    public class UpdateAgentCommandHandler : IRequestHandler<UpdateAgentCommand, Result<string>>
    {
        private readonly IAgentRepository _agentRepository;
    private readonly IMapper _mapper;

        public UpdateAgentCommandHandler(IAgentRepository agentRepository, IMapper mapper)
        {
            _agentRepository =  agentRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdateAgentCommand request, CancellationToken cancellationToken)
        {
            var agent = _mapper.Map<UpdateAgentCommand, Agent>(request);

            var rst = await _agentRepository.UpdateAgent(agent, cancellationToken);
            return rst > 0
                ? Result.Ok(agent.Id)
                : Result.Fail<string>("Not able to update agent now!");
        }
    }
}