using FluentValidation;

namespace Keis.Infrastructure.Features.Teams.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Department).NotEmpty();
    }
}