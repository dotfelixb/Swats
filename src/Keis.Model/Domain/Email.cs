using MassTransit;
using System.Text.Json.Serialization;

namespace Keis.Model.Domain;

public class Email : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public string Address { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string InHost { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EmailProtocol InProtocol { get; set; }
    public int InPort { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Encryption InSecurity { get; set; }
    public string OutHost { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EmailProtocol OutProtocol { get; set; }
    public int OutPort { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Encryption OutSecurity { get; set; }
    public string Note { get; set; }
}
