using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Tickets.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(r => r.Requester).NotEmpty().WithMessage("Requester must not be empty");
        RuleFor(r => r.Subject).NotEmpty().WithMessage("Subject must not be empty");
        RuleFor(r => r.Body).NotEmpty().WithMessage("Description must not be empty");
    }
}
