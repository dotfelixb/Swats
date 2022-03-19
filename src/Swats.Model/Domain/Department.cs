using MassTransit;

namespace Swats.Model.Domain;

public class Department : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Code { get; set; }
    public string Name { get; set; }
    public string Manager { get; set; }
    public string BusinessHour { get; set; }
    public string OutgoingEmail { get; set; }
    public DefaultType Type { get; set; }
    public string Response { get; set; }
}

public class Team : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; }
    public Guid Department { get; set; }
    public Guid Lead { get; set; }
}
