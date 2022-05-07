using MassTransit;

namespace Keis.Model.Domain;

public class WorkflowCriteria
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public CriteriaType Criteria { get; set; }
    public CriteriaCondition Condition { get; set; }
    public string Match { get; set; }
    public ControlType Control { get; set; }
    public string Link { get; set; }
}