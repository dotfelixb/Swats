using FluentValidation;

namespace Keis.Infrastructure.Features.TicketTypes.CreateTicketType;

public class CreateTicketTypeCommandValidator : AbstractValidator<CreateTicketTypeCommand>
{
    public CreateTicketTypeCommandValidator()
    {
        RuleFor(r => r.Name).NotNull();
    }
}