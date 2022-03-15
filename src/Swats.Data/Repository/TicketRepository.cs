using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Domain;

namespace Swats.Data.Repository;

public interface ITicketRepository
{
    Task<int> CreateTicket(Ticket ticket, CancellationToken cancellationToken);
    Task<long> GenerateTicketCode(CancellationToken cancellationToken);
}

public class TicketRepository : BasePostgresRepository, ITicketRepository
{
    public TicketRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<int> CreateTicket(Ticket ticket, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    public Task<long> GenerateTicketCode(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(conn =>
        {
            var query = "SELECT NEXTVAL('TicketCode');";

            return conn.QuerySingleAsync<long>(query);
        });
    }
}

