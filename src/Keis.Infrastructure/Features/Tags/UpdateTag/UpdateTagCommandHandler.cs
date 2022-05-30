using System.Text.Json;
using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Tags.UpdateTag;

public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public UpdateTagCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
    {
        var tag = _mapper.Map<UpdateTagCommand, Tag>(request);

        var rst = await _manageRepository.UpdateTag(tag, cancellationToken);
        return rst > 0
            ? Result.Ok(tag.Id)
            : Result.Fail<string>("Not able to update tag now!");
    }
}
