using System.Text.Json.Serialization;
using MassTransit;

namespace Keis.Model.Domain;

public class Department : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Code { get; set; }
    public string Name { get; set; }
    public string Manager { get; set; }
    public string BusinessHour { get; set; }
    public string OutgoingEmail { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DefaultType Type { get; set; }

    public string Response { get; set; }
}
