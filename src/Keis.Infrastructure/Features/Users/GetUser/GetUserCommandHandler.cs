using FluentResults;
using Keis.Data.Repository;
using Keis.Model.Commands;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Users.GetUser;

public class GetUserCommandHandler : IRequestHandler<GetUserCommand, Result<FetchUser>>
{
    private readonly IAuthUserRepository _authUserRepository;

    public GetUserCommandHandler(IAuthUserRepository authUserRepository)
    {
        _authUserRepository = authUserRepository;
    }

    public async Task<Result<FetchUser>> Handle(GetUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _authUserRepository.GetUser(request.Id, cancellationToken);

        return result is null
            ? Result.Fail<FetchUser>($"User with id [{request.Id}] does not exist!")
            : Result.Ok(result);
    }
}