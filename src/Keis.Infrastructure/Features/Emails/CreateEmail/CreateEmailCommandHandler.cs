using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Emails.CreateEmail;

public class CreateEmailCommandHandler : IRequestHandler<CreateEmailCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public CreateEmailCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }


    public async Task<Result<string>> Handle(CreateEmailCommand request, CancellationToken cancellationToken)
    {
        var email = _mapper.Map<CreateEmailCommand, Model.Domain.Email>(request);

        var rst = await _manageRepository.CreateEmail(email, cancellationToken);
        return rst > 0 
            ? Result.Ok(email.Id) : Result.Fail<string>("Not able to create now!");
    }
}
