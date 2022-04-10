using Keis.Model.Domain;

namespace Keis.Model.Queries;

public class FetchHelpTopic : HelpTopic
{
    public string DepartmentName { get; set; }
}