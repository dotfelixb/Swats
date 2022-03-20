using MassTransit;

namespace Swats.Model.Domain;

public class Tag : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Code { get; set; }
    public string Name { get; set; }
}

public class TicketTag : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Tag { get; set; }
}