namespace Swats.Model.Domain;

public class Agent : DbAudit
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Mobile { get; set; }
    public string Telephone { get; set; }
    public Guid Timezone { get; set; }
    public Guid Department { get; set; }
    public Guid Team { get; set; }
}