using Swats.Model.Domain;

namespace Swats.Model.Queries;

internal class AdminManageQuery
{
}

public class FetchTicketType
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
}