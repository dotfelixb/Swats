using FluentResults;
using Keis.Model.Queries;
using MassTransit;
using MediatR;

namespace Keis.Model.Commands;

public class LoginCommand
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    public string Source { get; set; } = "KeisApp";
}

public class LoginLogCommand : IRequest<Result>
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string AuthUser { get; set; }
    public string Device { get; set; }
    public string Platform { get; set; }
    public string Browser { get; set; }
    public string Address { get; set; }
}

public class GetUserCommand : GetType, IRequest<Result<FetchUser>>
{
}