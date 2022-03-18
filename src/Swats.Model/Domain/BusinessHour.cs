using MassTransit;

namespace Swats.Model.Domain;

public class BusinessHour : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public DefaultStatus Status { get; set; }
    public string[] Holidays { get; set; }
}