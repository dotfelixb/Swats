using MassTransit;

namespace Swats.Model.Domain;

public class Sla : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Hours { get; set; }
}

public class TicketSla : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Sla { get; set; }
}