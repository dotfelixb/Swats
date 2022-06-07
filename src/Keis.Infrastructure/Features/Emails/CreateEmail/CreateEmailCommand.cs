using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Emails.CreateEmail;

public class CreateEmailCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public string InHost { get; set; }
    public EmailProtocol InProtocol { get; set; }
    public int InPort { get; set; }
    public Encryption InSecurity { get; set; }

    public string OutHost { get; set; }
    public EmailProtocol OutProtocol { get; set; }
    public int OutPort { get; set; }
    public Encryption OutSecurity { get; set; }

    public DefaultStatus Status { get; set; }
    public string Note { get; set; }
    public string CreatedBy { get; set; }
}
