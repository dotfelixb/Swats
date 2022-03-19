using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.TicketTypes.CreateTicketType;

public class CreateTicketTypeCommandValidator : AbstractValidator<CreateTicketTypeCommand>
{
    public CreateTicketTypeCommandValidator()
    {
        RuleFor(r => r.Name).NotNull();
    }
}

