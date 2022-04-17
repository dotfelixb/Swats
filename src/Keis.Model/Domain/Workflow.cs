using MassTransit;

namespace Keis.Model.Domain;

public class Workflow : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public WorkflowEvent[] Events { get; set; }
    
}