using FluentValidation;

namespace Keis.Infrastructure.Features.Emails.UpdateEmail;

public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{
    public UpdateEmailCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty(); 

        RuleFor(r => r.Name).NotEmpty();
        RuleFor(r => r.Address).NotEmpty();
        RuleFor(r => r.Username).NotEmpty();
        RuleFor(r => r.Password).NotEmpty();

        RuleFor(r => r.InHost).NotEmpty();
        RuleFor(r => r.InProtocol).NotEmpty();
        RuleFor(r => r.InPort).NotEmpty();
        RuleFor(r => r.InSecurity).NotEmpty();

        RuleFor(r => r.OutHost).NotEmpty();
        RuleFor(r => r.OutProtocol).NotEmpty();
        RuleFor(r => r.OutPort).NotEmpty();
        RuleFor(r => r.OutSecurity).NotEmpty();
    }
}
