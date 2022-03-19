using MassTransit;

namespace Swats.Model.Domain;

public class Department : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Code { get; set; }
    public string Name { get; set; }
}

public class Team : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; }
    public Guid Department { get; set; }
    public Guid Lead { get; set; }
}
