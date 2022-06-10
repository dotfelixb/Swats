using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Template.CreateTemplate;

internal class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateTemplateCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }


    public async Task<Result<string>> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = _mapper.Map<CreateTemplateCommand, Model.Domain.Template>(request);

        var rst = await _manageRepository.CreateTemplate(template, cancellationToken);
        return rst > 0
            ? Result.Ok(template.Id)
            : Result.Fail<string>("Not able to create now!");
    }
}
