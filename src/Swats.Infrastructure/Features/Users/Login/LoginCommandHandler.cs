using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Users.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(r => r.UserName).NotEmpty();
        RuleFor(r => r.Password).NotEmpty();
    }
}

public class LoginCommandHandler
{
}