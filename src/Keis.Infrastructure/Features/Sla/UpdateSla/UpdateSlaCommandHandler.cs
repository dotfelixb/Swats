using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Sla.UpdateSla;

public class UpdateSlaCommandHandler : IRequestHandler<UpdateSlaCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public UpdateSlaCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateSlaCommand request, CancellationToken cancellationToken)
    {
        var sla = _mapper.Map<UpdateSlaCommand, Model.Domain.Sla>(request);

        var rst = await _manageRepository.UpdateSla(sla, cancellationToken);

        return rst > 0
            ? Result.Ok(sla.Id)
            : Result.Fail<string>("Not able to update sla now!");
    }
}
