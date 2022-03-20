using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Data.Repository;

public interface IAgentRepository
{
    Task<int> CreateAgent(Agent agent, DbAuditLog audit, CancellationToken cancellationToken);
    Task<FetchedAgent> GetAgent(string id, CancellationToken cancellationToken);
    Task<IEnumerable<FetchedAgent>> ListAgent(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default);
}

public class AgentReposiory : BasePostgresRepository, IAgentRepository
{
    public AgentReposiory(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<int> CreateAgent(Agent agent, DbAuditLog audit, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<FetchedAgent> GetAgent(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
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
