using FluentResults;
using MassTransit;
using MediatR;

namespace Swats.Model.Commands;

public class LoginCommand
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    public string Source { get; set; } = "SwatsApp";
}

public class LoginLogCommand : IRequest<Result> {
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string AuthUser { get; set; }
    public string Device { get; set; }
    public string Platform { get; set; }
    public string Browser { get; set; }
    public string Address { get; set; }
}