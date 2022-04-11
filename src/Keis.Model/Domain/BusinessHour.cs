using MassTransit;

namespace Keis.Model.Domain;

public class BusinessHour : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public string[] Holidays { get; set; }
    public OpenHour[] OpenHours { get; set; }
}

public class OpenHour
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public bool FullDay { get; set; }
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
}