using Swats.Model.Domain;

namespace Swats.Model.Queries;

public class FetchDepartment : Department
{
}

public class FetchTeam : Team
{
    public string DepartmentName { get; set; }
}

