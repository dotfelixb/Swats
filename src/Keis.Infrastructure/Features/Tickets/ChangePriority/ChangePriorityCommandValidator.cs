using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.ChangePriority;

public class ChangePriorityCommandValidator:AbstractValidator<ChangePriorityCommand>
{
    public ChangePriorityCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Priority).NotNull().IsInEnum();
    }
}
