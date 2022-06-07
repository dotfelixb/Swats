using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using MediatR;

namespace Keis.Infrastructure.Features.Emails.UpdateEmail;

internal class UpdateEmailCommandHandler : IRequestHandler<UpdateEmailCommand, Result<string>>
{
    private readonly IManageRepository _manageRepository;
    private readonly IMapper _mapper;

    public UpdateEmailCommandHandler(IManageRepository manageRepository, IMapper mapper)
    {
        _manageRepository = manageRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(UpdateEmailCommand request, CancellationToken cancellationToken)
    {
        var email = _mapper.Map<UpdateEmailCommand, Model.Domain.Email>(request);

        var rst = await _manageRepository.UpdateEmail(email, cancellationToken);

        return rst > 0
            ? Result.Ok(email.Id) : Result.Fail<string>("Not able to update email now!");
    }
}
