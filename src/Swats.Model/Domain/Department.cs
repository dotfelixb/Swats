using MassTransit;
using System.Text.Json.Serialization;

namespace Swats.Model.Domain;

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

public class Team : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Department { get; set; }
    public string Lead { get; set; }
    public string Note { get; set; }
}