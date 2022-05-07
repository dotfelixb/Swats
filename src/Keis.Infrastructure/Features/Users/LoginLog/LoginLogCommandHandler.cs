using AutoMapper;
using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Users.LoginLog;

public class LoginLogCommandHandler : IRequestHandler<LoginLogCommand, Result>
{
    private readonly IAuthUserRepository _authUserRepository;
    private readonly IMapper _mapper;

    public LoginLogCommandHandler(IAuthUserRepository authUserRepository, IMapper mapper)
    {
        _authUserRepository = authUserRepository;
        _mapper = mapper;
    }

    public async Task<Result> Handle(LoginLogCommand request, CancellationToken cancellationToken)
    {
        var loginAudit = _mapper.Map<LoginLogCommand, LoginAudit>(request);

        await _authUserRepository.WriteLoginAuditAsync(loginAudit, cancellationToken);

        return Result.Ok();
    }
}