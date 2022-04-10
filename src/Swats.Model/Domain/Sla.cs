using MassTransit;

namespace Swats.Model.Domain;

public class Sla : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public string BusinessHour { get; set; }
    public int ResponsePeriod { get; set; }
    public DefaultTimeFormat ResponseFormat { get; set; }
    public bool ResponseNotify { get; set; }
    public bool ResponseEmail { get; set; }
    public int ResolvePeriod { get; set; }
    public DefaultTimeFormat ResolveFormat { get; set; }
    public bool ResolveNotify { get; set; }
    public bool ResolveEmail { get; set; }
}