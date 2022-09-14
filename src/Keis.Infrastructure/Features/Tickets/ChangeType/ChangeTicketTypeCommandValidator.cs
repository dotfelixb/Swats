using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.ChangeType;

public class ChangeTicketTypeCommandValidator : AbstractValidator<ChangeTicketTypeCommand>
{
    public ChangeTicketTypeCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.TicketType).NotEmpty();
    }
}
