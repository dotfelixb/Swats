using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Data.Repository;

public interface IManageRepository
{
    Task<int> CreateBusinessHour(BusinessHour businessHour, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchBusinessHour> GetBusinessHour(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchBusinessHour>> ListBusinessHours(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default);
  
    Task<int> CreateTag(Tag tag, DbAuditLog auditLog, CancellationToken cancellationToken);
   
    Task<FetchTag> GetTag(Guid id, CancellationToken cancellationToken);
    
    Task<IEnumerable<FetchTag>> ListTags(int offset, int limit, bool deleted, CancellationToken cancellationToken);
}

public class ManageRepository : BasePostgresRepository, IManageRepository
{
    public ManageRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    public Task<int> CreateBusinessHour(BusinessHour businessHour, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn => {
            var cmd = @"
                INSERT INTO public.businesshour 
	                (id
                    , ""name""
                    , description
                    , timezone
                    , status
                    , rowversion
                    , createdby
                    , updatedby) 
                VALUES(
                    @Id
                    , @Name
                    , @Description
                    , @Timezone
                    , @Status
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy);";

            var crst = await conn.ExecuteAsync(cmd, new
            {
                businessHour.Id,
                businessHour.Name,
                businessHour.Description,
                businessHour.Timezone,
                businessHour.Status,
                businessHour.RowVersion,
                businessHour.CreatedBy,
                businessHour.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.businesshourauditlog
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

            var lrst = await conn.ExecuteAsync(logCmd, new
            {
                auditLog.Id,
                auditLog.Target,
                auditLog.ActionName,
                auditLog.Description,
                auditLog.ObjectName,
                auditLog.ObjectData,
                auditLog.CreatedBy
            });

            return crst + lrst;
        });
    }

    public Task<int> CreateTag(Tag tag, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<FetchBusinessHour> GetBusinessHour(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT b.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.updatedby) AS UpdatedByName
                FROM businesshour b
                WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchBusinessHour>(query, new { Id = id });
        });
    }

    public Task<FetchTag> GetTag(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FetchBusinessHour>> ListBusinessHours(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND b.deleted = FALSE ";
            var query = $@"
                SELECT b.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = b.updatedby) AS UpdatedByName
                FROM businesshour b
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchBusinessHour>(query, new { offset, limit });
        });
    }

    public Task<IEnumerable<FetchTag>> ListTags(int offset, int limit, bool deleted, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
