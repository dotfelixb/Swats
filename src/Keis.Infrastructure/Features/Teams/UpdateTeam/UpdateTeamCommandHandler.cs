using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Teams.UpdateTeam
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand, Result<string>>
    {
        private readonly IManageRepository _manageRepository;
        private readonly IMapper _mapper;

        public UpdateTeamCommandHandler(IManageRepository manageRepository, IMapper mapper)
        {
            _manageRepository = manageRepository;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = _mapper.Map<UpdateTeamCommand, Keis.Model.Domain.Team>(request);

            var rst = await _manageRepository.UpdateTeam(team, cancellationToken);
            return rst > 0
                ? Result.Ok(team.Id)
                : Result.Fail<string>("Not able to update team now!");
        }
    }
}