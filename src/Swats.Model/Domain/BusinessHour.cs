using MassTransit;

namespace Swats.Model.Domain;

public class BusinessHour : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public string[] Holidays { get; set; }
}