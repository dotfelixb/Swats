using FluentValidation;
using Keis.Model.Commands;

namespace Keis.Infrastructure.Features.Tickets.GetTicket;

public class GetTicketCommandValidator : AbstractValidator<GetTicketCommand>
{
    public GetTicketCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("specify a ticket id");
    }
}
