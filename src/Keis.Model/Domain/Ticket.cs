using System.Text.Json.Serialization;
using MassTransit;

namespace Keis.Model.Domain;

public class Ticket : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Code { get; set; }
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string ExternalAgent { get; set; }
    public string AssignedTo { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TicketSource Source { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public new TicketStatus Status { get; set; }

    public string TicketType { get; set; }
    public string Department { get; set; }
    public string Team { get; set; }
    public string Sla { get; set; }
    public string HelpTopic { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TicketPriority Priority { get; set; }

    public DateTimeOffset DueAt { get; set; }
}

public class TicketType : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DefaultType Visibility { get; set; }
}

public class TicketTypeAuditLog : DbAuditLog
{
}

public class TicketComment : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Ticket { get; set; }
    public string FromEmail { get; set; }
    public string FromName { get; set; }
    public string ToEmail { get; set; }
    public string ToName { get; set; }
    public string[][] Receiptients { get; set; }
    public string Body { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CommentType Type { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TicketSource Source { get; set; }

    public string Target { get; set; }
}

public class TicketFellow : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Fellow { get; set; }
    public int Points { get; set; }
    public bool UnFollow { get; set; }
}