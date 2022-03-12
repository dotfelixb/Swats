using Microsoft.Extensions.Options;
using Swats.Model;

namespace Swats.Data.Repository;

public interface ITicketRepository
{
}

internal class TicketRepository : BasePostgresRepository, ITicketRepository
{
    public TicketRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }
}