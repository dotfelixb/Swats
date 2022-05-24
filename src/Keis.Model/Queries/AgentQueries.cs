using Keis.Model.Domain;

namespace Keis.Model.Queries;

public class FetchAgent : Agent
{
    public string DepartmentName { get; set; }
    public string TeamName { get; set; }
    public string TicketTypeName { get; set; }
}