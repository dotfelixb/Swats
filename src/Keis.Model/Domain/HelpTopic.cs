using System.Text.Json.Serialization;
using MassTransit;

namespace Keis.Model.Domain;

public class HelpTopic : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Topic { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DefaultType Type { get; set; }
    public string Department { get; set; }
    public string DefaultDepartment { get; set; }
    public string Note { get; set; }
}

public class HelpTopicAuditLog : DbAuditLog
{
}