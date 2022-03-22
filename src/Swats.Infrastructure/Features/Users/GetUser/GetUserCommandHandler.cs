using FluentResults;
using MediatR;
using Swats.Data.Repository;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Queries;

namespace Swats.Infrastructure.Features.Users.GetUser;

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
      
        return result.Id.ToGuid() == Guid.Empty
            ? Result.Fail<FetchUser>($"User with id [{request.Id}] does not exist!")
            : Result.Ok(result);
    }
}