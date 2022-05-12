using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.ChangeStatus;

public class ChangeStatusCommandValidator : AbstractValidator<ChangeStatusCommand>
{
    public ChangeStatusCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Status).NotNull();
    }
}

