using Swats.Model.Domain;

namespace Swats.Model.Queries;

public class FetchDepartment : Department
{
    public string ManagerName { get; set; }
    public string BusinessHourName { get; set; }
}

public class FetchTeam : Team
{
    public string DepartmentName { get; set; }
    public string LeadName { get; set; }
}