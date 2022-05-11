using FluentValidation;

namespace Keis.Infrastructure.Features.Tickets.CreateComment;

public class CreateTicketCommentCommandValidator : AbstractValidator<CreateTicketCommentCommand>
{
    public CreateTicketCommentCommandValidator()
    {
        RuleFor(r => r.TicketId).NotEmpty();
        RuleFor(r => r.Body).NotEmpty();
    }
}