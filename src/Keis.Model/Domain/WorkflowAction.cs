using MassTransit;

namespace Keis.Model.Domain;

public class WorkflowAction
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; }
    public ActionType ActionFrom { get; set; }
    public string ActionTo { get; set; }
    public ControlType Control { get; set; }
    public string Link { get; set; }
}