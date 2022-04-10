using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Tickets.GetTicket;

public class GetTicketCommandValidator : AbstractValidator<GetTicketCommand>
{
    public GetTicketCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("specify a ticket id");
    }
}
