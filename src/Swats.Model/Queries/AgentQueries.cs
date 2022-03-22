using Swats.Model.Domain;

namespace Swats.Model.Queries;

public class FetchAgent : Agent
{
    public string DepartmentName { get; set; }
    public string TeamName { get; set; }
    public string TypeName { get; set; }
}