using System.Text.Json.Serialization;
using MassTransit;

namespace Keis.Model.Domain;

public class Tag : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Color { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DefaultType Visibility { get; set; }

    public string Note { get; set; }
}

public class TicketTag : DbAudit
{
    public Guid Id { get; set; } = NewId.NextGuid();
    public Guid Ticket { get; set; }
    public Guid Tag { get; set; }
}
