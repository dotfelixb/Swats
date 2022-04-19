using Dapper;
using Keis.Model;
using Keis.Model.Domain;
using Keis.Model.Queries;
using Microsoft.Extensions.Options;

namespace Keis.Data.Repository;

public interface IAgentRepository
{
    Task<int> CreateAgent(Agent agent, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchAgent> GetAgent(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchAgent>> ListAgent(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);
}

public class AgentReposiory : BasePostgresRepository, IAgentRepository
{
    public AgentReposiory(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<int> CreateAgent(Agent agent, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        return WithConnection(async conn =>
        {
            var cmd = @"
                INSERT INTO public.agent
                    (id
                    , email
                    , firstname
                    , lastname
                    , mobile
                    , telephone
                    , timezone
                    , department
                    , team
                    , tickettype
                    , status
                    , ""mode""
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES(@Id
                    , @Email
                    , @FirstName
                    , @LastName
                    , @Mobile
                    , @Telephone
                    , @Timezone
                    , @Department
                    , @Team
                    , @Type
                    , @Status
                    , @Mode
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy);
                ";

            var ctt = await conn.ExecuteAsync(cmd, new
            {
                agent.Id,
                agent.Email,
                agent.FirstName,
                agent.LastName,
                agent.Mobile,
                agent.Telephone,
                agent.Timezone,
                agent.Department,
                agent.Team,
                agent.Type,
                agent.Status,
                agent.Mode,
                agent.RowVersion,
                agent.CreatedBy,
                agent.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.agentauditlog
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

    public Task<FetchAgent> GetAgent(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT g.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.updatedby) AS UpdatedByName
	                , d.""name"" AS departmentname
	                , t.""name"" AS teamname
                    , tt.""name"" AS typename
                FROM agent g
                LEFT JOIN department d ON d.id = g.department
                LEFT JOIN team t ON t.id = g.team
                LEFT JOIN tickettype tt ON tt.id = g.tickettype
                WHERE g.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchAgent>(query, new { Id = id });
        });
    }

    public Task<IEnumerable<FetchAgent>> ListAgent(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND g.deleted = FALSE ";
            var query = $@"
                SELECT g.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.updatedby) AS UpdatedByName
	                , d.""name"" AS departmentname
	                , t.""name"" AS teamname
                    , tt.""name"" AS typename
                FROM agent g
                LEFT JOIN department d ON d.id = g.department
                LEFT JOIN team t ON t.id = g.team
                LEFT JOIN tickettype tt ON tt.id = g.tickettype
                WHERE 1 = 1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchAgent>(query, new { offset, limit });
        });
    }
}