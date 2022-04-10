using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Agents.CreateAgent;

public class CreateAgentCommandValidator : AbstractValidator<CreateAgentCommand>
{
    public CreateAgentCommandValidator()
    {
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
        RuleFor(r => r.Mobile).NotEmpty();
        RuleFor(r => r.FirstName).NotEmpty();
        RuleFor(r => r.LastName).NotEmpty();
        RuleFor(r => r.Mode).NotNull();
    }
}