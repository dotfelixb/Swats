using FluentValidation;
using Swats.Model.Commands;

namespace Swats.Infrastructure.Features.Teams.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Department).NotEmpty();
    }
}