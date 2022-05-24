using Dapper;
using Keis.Model;
using Keis.Model.Domain;
using Keis.Model.Queries;
using Microsoft.Extensions.Options;

namespace Keis.Data.Repository;

public interface IAgentRepository
{
    Task<int> CreateAgent(Agent agent, CancellationToken cancellationToken);

    Task<FetchAgent> GetAgent(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchAgent>> ListAgent(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    Task<int> UpdateAgent(Agent agent, CancellationToken cancellationToken);
}

public class AgentReposiory : BasePostgresRepository, IAgentRepository
{
    public AgentReposiory(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<int> CreateAgent(Agent agent, CancellationToken cancellationToken)
    {
        return WithConnection(conn =>
        {
            var cmd = @"
                WITH inserted_agent AS(
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
                    , mode
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
                    , @TicketType
                    , @Status
                    , @Mode
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy)
                RETURNING *
                )
                INSERT INTO public.agentauditlog
                    (id
                    , target
                    , actionname
                    , description
                    , objectname
                    , objectdata
                    , createdby)
                SELECT uuid_generate_v1()
                    , id
                    , 'agent.create'
                    , 'added an agent'
                    , 'agent'
                    , row_to_json(inserted_agent)
                    , createdby
                FROM inserted_agent";

            return conn.ExecuteAsync(cmd, new
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
                agent.TicketType,
                agent.Status,
                agent.Mode,
                agent.RowVersion,
                agent.CreatedBy,
                agent.UpdatedBy
            });
        });
    }

    public Task<FetchAgent> GetAgent(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT g.*
                    , CONCAT(g.firstname, ' ', g.lastname) as ""name"" 
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.updatedby) AS UpdatedByName
	                , d.""name"" AS departmentname
	                , t.""name"" AS teamname
                    , tt.""name"" AS tickettypename
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
                    , CONCAT(g.firstname, ' ', g.lastname) as ""name"" 
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = g.updatedby) AS UpdatedByName
	                , d.""name"" AS departmentname
	                , t.""name"" AS teamname
                    , tt.""name"" AS tickettypename
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

    public Task<int> UpdateAgent(Agent agent, CancellationToken cancellationToken)
    {
        return WithConnection( conn =>
        {
            var cmd = @"
                WITH changed_agent AS (
                    UPDATE public.agent
                    SET email = @Email
                        , firstname = @FirstName
                        , lastname =  @LastName
                        , mobile = @Mobile
                        , telephone = @Telephone
                        , timezone = @Timezone
                        , department = @Department
                        , team = @Team
                        , tickettype = @TicketType
                        , status = @Status
                        , mode = @Mode
                        , note = @Note
                        , rowversion = @RowVersion
                        , updatedby = @UpdatedBy
                        , updatedat = now()
                    WHERE id = @Id
                    RETURNING *
                )
                INSERT INTO public.agentauditlog
                    (id
                    , target
                    , actionname
                    , description
                    , objectname
                    , objectdata
                    , createdby)
                SELECT uuid_generate_v1()
                    , id
                    , 'agent.update'
                    , 'updated agent'
                    , 'agent'
                    , row_to_json(changed_agent)
                    , updatedby
                FROM changed_agent
                ";

            return conn.ExecuteAsync(cmd, new
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
                agent.TicketType,
                agent.Status,
                agent.Mode,
                agent.Note,
                agent.RowVersion,
                agent.UpdatedBy
            });
        });
    }
}