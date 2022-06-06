using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.ChangeDue;

public class ChangeDueCommandValidator : AbstractValidator<ChangeDueCommand>
    {
        public ChangeDueCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.DueAt).NotNull();
        }
    }
