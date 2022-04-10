using FluentValidation;
using Keis.Model.Commands;

namespace Keis.Infrastructure.Features.Users.Login;

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