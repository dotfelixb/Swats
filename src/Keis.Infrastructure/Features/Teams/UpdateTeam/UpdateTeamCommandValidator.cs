using FluentValidation;

namespace Keis.Infrastructure.Features.Teams.UpdateTeam
{
    public class UpdateTeamCommandValidator: AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty();
        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Department).NotEmpty();
    }
}
}