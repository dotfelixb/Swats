using MassTransit;

namespace Keis.Model.Domain;

public class Workflow : DbAudit
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public WorkflowEvent[] Events { get; set; }
    public WorkflowCriteria[] Criterias { get; set; }
    public WorkflowAction[] Actions { get; set; }
}

public class WorkflowCriteria
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public CriteriaType Criteria { get; set; }
    public CriteriaCondition Condition { get; set; }
    public string Match { get; set; }
}

public class WorkflowAction
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public ActionType ActionFrom { get; set; }
    public string ActionTo { get; set; }
}