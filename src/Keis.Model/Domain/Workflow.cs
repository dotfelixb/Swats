using System.Text.Json.Serialization;
using MassTransit;

namespace Keis.Model.Domain;

public class Workflow : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public EventType[] Events { get; set; }
    public WorkflowCriteria[] Criteria { get; set; }
    public WorkflowAction[] Actions { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WorkflowPriority Priority { get; set; }
    public string Note { get; set; }
}