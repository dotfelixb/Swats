using Keis.Model.Domain;

namespace Keis.Model.Queries;

public class FetchSla : Sla
{
    public string BusinessHourName { get; set; }
}

public class FetchWorkflow : Workflow
{ }