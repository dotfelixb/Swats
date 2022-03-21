using MassTransit;

namespace Swats.Model.Domain;

public class Ticket : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Code { get; set; }
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string Body { get; set; }
    public string ExternalAgent { get; set; }
    public string AssignedTo { get; set; }
    public TicketSource Source { get; set; }
    public string TicketType { get; set; }
    public string Department { get; set; }
    public string Team { get; set; }
    public string HelpTopic { get; set; }
    public TicketPriority Priority { get; set; }
}

public class TicketType : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
}

public class TicketTypeAuditLog : DbAuditLog { }

public class TicketFellow : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Fellow { get; set; }
    public int Points { get; set; }
    public bool UnFollow { get; set; }
}
