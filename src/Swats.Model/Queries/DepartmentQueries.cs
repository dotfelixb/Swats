using Swats.Model.Domain;

namespace Swats.Model.Queries;

public class FetchDepartment : Department
{
    public string BusinessHourName { get; set; }
}

public class FetchTeam : Team
{
    public string DepartmentName { get; set; }
}

