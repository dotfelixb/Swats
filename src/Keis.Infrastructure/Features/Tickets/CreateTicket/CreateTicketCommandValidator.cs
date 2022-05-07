using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.CreateTicket;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(r => r.Requester).NotEmpty().WithMessage("Requester must not be empty");
        RuleFor(r => r.Subject).NotEmpty().WithMessage("Subject must not be empty");
        RuleFor(r => r.Body).NotEmpty().WithMessage("Description must not be empty");

        RuleFor(r => r.AssignedTo).NotNull().When(r => r.Department == null && r.Team == null && r.HelpTopic == null);
        RuleFor(r => r.Department).NotNull().When(r => r.AssignedTo == null && r.Team == null && r.HelpTopic == null);
        RuleFor(r => r.Team).NotNull().When(r => r.Department == null && r.AssignedTo == null && r.HelpTopic == null);
        RuleFor(r => r.HelpTopic).NotNull().When(r => r.Department == null && r.Team == null && r.AssignedTo == null);
    }
}