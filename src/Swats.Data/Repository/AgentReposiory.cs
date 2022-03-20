using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Data.Repository;

public interface IAgentRepository
{
    Task<int> CreateAgent(Agent agent, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchedAgent> GetAgent(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchedAgent>> ListAgent(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default);
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

    public Task<FetchedAgent> GetAgent(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT b.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.updatedby) AS UpdatedByName
                FROM agent b
                WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchedAgent>(query, new { Id = id });
        });
    }

    public Task<IEnumerable<FetchedAgent>> ListAgent(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND b.deleted = FALSE ";
            var query = $@"
                SELECT b.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.updatedby) AS UpdatedByName
                FROM agent b
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchedAgent>(query, new { offset, limit });
        });
    }
}