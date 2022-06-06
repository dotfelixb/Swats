using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.AssignTicket;

public class AssignTicketCommandValidator : AbstractValidator<AssignTicketCommand>
{
    public AssignTicketCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.AssignedTo).NotEmpty();
    }
}