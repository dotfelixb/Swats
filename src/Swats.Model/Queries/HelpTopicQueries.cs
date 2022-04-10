using Swats.Model.Domain;

namespace Swats.Model.Queries;

public class FetchHelpTopic : HelpTopic
{
    public string DepartmentName { get; set; }
}