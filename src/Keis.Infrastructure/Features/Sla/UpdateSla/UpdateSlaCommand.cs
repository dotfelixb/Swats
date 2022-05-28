using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.Sla.UpdateSla;

public class UpdateSlaCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public string BusinessHour { get; set; }
    public int ResponsePeriod { get; set; }
    public DefaultTimeFormat ResponseFormat { get; set; }
    public bool ResponseNotify { get; set; }
    public bool ResponseEmail { get; set; }
    public int ResolvePeriod { get; set; }
    public DefaultTimeFormat ResolveFormat { get; set; }
    public bool ResolveNotify { get; set; }
    public bool ResolveEmail { get; set; }
    public DefaultStatus Status { get; set; }
    public string UpdatedBy { get; set; }
}
