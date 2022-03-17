using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;

namespace Swats.Data.Repository;

public interface ITicketRepository
{
    Task<long> GenerateTicketCode(CancellationToken cancellationToken);
    
    Task<int> CreateTicket(Ticket ticket, CancellationToken cancellationToken);

    Task<int> CreateTicketType(TicketType ticketType, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<TicketType> GetTicketType(Guid id, CancellationToken cancellationToken);

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

    public Task<int> CreateTicketType(TicketType ticketType, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var cmd = $@"
                INSERT INTO public.tickettype
                    (id
                    , ""name""
                    , description
                    , color
                    , visibility
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES
                    (@Id
                    , @Name
                    , @Description
                    , @Color
                    , @Visibility
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy);
                ";

            var ctt = await conn.ExecuteAsync(cmd, new
            {
                ticketType.Id,
                ticketType.Name,
                ticketType.Description,
                ticketType.Color,
                ticketType.Visibility,
                ticketType.RowVersion,
                ticketType.CreatedBy,
                ticketType.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.tickettypeauditlog
                    (id
                    , target
                    , actionname
                    , description
                    , objectname
                    , objectdata
                    , createdby)
                VALUES
                    (@Id
                    , @Target
                    , @ActionName
                    , @Description
                    , @ObjectName
                    , @ObjectData::jsonb
                    , @CreatedBy);
                ";

            var cl = await conn.ExecuteAsync(logCmd, new
            {
                auditLog.Id,
                auditLog.Target,
                auditLog.ActionName,
                auditLog.Description,
                auditLog.ObjectName,
                auditLog.ObjectData,
                auditLog.CreatedBy
            });

            return ctt + cl;
        });
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

    public Task<TicketType> GetTicketType(Guid id, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var query = @"SELECT * FROM public.tickettype WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<TicketType>(query, new { Id = id});
        });
    }
}