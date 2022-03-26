using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Tickets.CreateComment;

public class CreateTicketCommentCommandValidator : AbstractValidator<CreateTicketCommentCommand>
{
    public CreateTicketCommentCommandValidator()
    {
        RuleFor(r => r.Body).NotNull();
    }
}