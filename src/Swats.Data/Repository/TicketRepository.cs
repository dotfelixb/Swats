using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Data.Repository;

public interface ITicketRepository
{
    #region Ticket

    Task<long> GenerateTicketCode(CancellationToken cancellationToken);

    Task<int> CreateTicket(Ticket ticket, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchTicket> GetTicket(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchTicket>> ListTickets(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    Task<int> CountByAgentId(string id, CancellationToken cancellationToken);

    #endregion Ticket

    #region Ticket Type

    Task<int> CreateTicketType(TicketType ticketType, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchTicketType> GetTicketType(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchTicketType>> ListTicketTypes(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    #endregion Ticket Type
}

public class TicketRepository : BasePostgresRepository, ITicketRepository
{
    public TicketRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    #region Ticket

    public Task<int> CreateTicket(Ticket ticket, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var cmd = @"
                    INSERT INTO public.ticket
                        (id
                        , code
                        , subject
                        , requester
                        , body
                        , externalagent
                        , assignedto
                        , ""source""
                        , tickettype
                        , department
                        , team
                        , helptopic
                        , priority
                        , status
                        , rowversion
                        , createdby
                        , updatedby)
                    VALUES(
                        @Id
                        , @Code
                        , @Subject
                        , @Requester
                        , @Body
                        , @ExternalAgent
                        , @AssignedTo
                        , @Source
                        , @TicketType
                        , @Department
                        , @Team
                        , @HelpTopic
                        , @Priority
                        , @Status
                        , @RowVersion
                        , @CreatedBy
                        , @UpdatedBy);
                    ";

            var ctt = await conn.ExecuteAsync(cmd, new
            {
                ticket.Id,
                ticket.Code,
                ticket.Subject,
                ticket.Requester,
                ticket.Body,
                ticket.ExternalAgent,
                ticket.AssignedTo,
                ticket.Source,
                ticket.TicketType,
                ticket.Department,
                ticket.Team,
                ticket.HelpTopic,
                ticket.Priority,
                ticket.Status,
                ticket.RowVersion,
                ticket.CreatedBy,
                ticket.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.ticketauditlog
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

    public Task<FetchTicket> GetTicket(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                    , d.""name"" AS departmentname
                    , m.""name"" as teamname
                    , tt.""name"" as tickettypename
                    , h.topic as helptopicname
                    , CONCAT(r.firstname, ' ', r.lastname) as requestername
                    , CONCAT(g.firstname, ' ', g.lastname) as assignedtoname
                FROM ticket t
                LEFT JOIN department d ON d.id = t.department
                LEFT JOIN team m on m.id = t.team
                LEFT JOIN agent g on g.id = t.assignedto
                LEFT JOIN tickettype tt on tt.id = t.tickettype
                LEFT JOIN helptopic h on h.id = t.helptopic
                LEFT JOIN agent r on r.id = t.requester
                WHERE t.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchTicket>(query, new { Id = id });
        });
    }

    public Task<IEnumerable<FetchTicket>> ListTickets(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND t.deleted = FALSE ";
            var query = $@"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                    , d.""name"" AS departmentname
                    , m.""name"" as teamname
                    , tt.""name"" as tickettypename
                    , h.topic as helptopicname
                    , CONCAT(r.firstname, ' ', r.lastname) as requestername
                    , CONCAT(g.firstname, ' ', g.lastname) as assignedtoname
                FROM ticket t
                LEFT JOIN department d ON d.id = t.department
                LEFT JOIN team m on m.id = t.team
                LEFT JOIN agent g on g.id = t.assignedto
                LEFT JOIN tickettype tt on tt.id = t.tickettype
                LEFT JOIN helptopic h on h.id = t.helptopic
                LEFT JOIN agent r on r.id = t.requester
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchTicket>(query, new { offset, limit });
        });
    }

    public Task<int> CountByAgentId(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT COUNT(t.Id)
                FROM ticket t
                WHERE t.assignedto = @Id";

            return await conn.ExecuteScalarAsync<int>(query, new { Id = id });
        });
    }

    #endregion Ticket

    #region Ticket Type

    public Task<int> CreateTicketType(TicketType ticketType, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var cmd = @"
                INSERT INTO public.tickettype
                    (id
                    , ""name""
                    , description
                    , color
                    , visibility
                    , status
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES
                    (@Id
                    , @Name
                    , @Description
                    , @Color
                    , @Visibility
                    , @Status
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
                ticketType.Status,
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

    public Task<FetchTicketType> GetTicketType(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                FROM tickettype t
                WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchTicketType>(query, new { Id = id });
        });
    }

    public Task<IEnumerable<FetchTicketType>> ListTicketTypes(int offset = 0, int limit = 1000,
        bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND t.deleted = FALSE ";
            var query = $@"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                FROM tickettype t
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchTicketType>(query, new { offset, limit });
        });
    }

    #endregion Ticket Type
}