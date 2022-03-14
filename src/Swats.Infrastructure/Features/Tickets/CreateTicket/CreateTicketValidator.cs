using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Tickets.CreateTicket;

public class CreateTicketValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketValidator()
    {
        RuleFor(r => r.Requester).NotEmpty();
        RuleFor(r => r.Subject).NotEmpty();
        RuleFor(r => r.Priority).NotEmpty();
        RuleFor(r => r.Body).NotEmpty();
    }
}
