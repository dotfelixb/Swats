using Dapper;
using Microsoft.Extensions.Options;
using Swats.Model;
using Swats.Model.Domain;
using Swats.Model.Queries;

namespace Swats.Data.Repository;

public interface IManageRepository
{
    #region Business Hour

    Task<int> CreateBusinessHour(BusinessHour businessHour, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchBusinessHour> GetBusinessHour(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchBusinessHour>> ListBusinessHours(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default);

    #endregion Business Hour

    #region Tags

    Task<int> CreateTag(Tag tag, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchTag> GetTag(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchTag>> ListTags(int offset, int limit, bool includeDeleted = false, CancellationToken cancellationToken = default);

    #endregion Tags

    #region Department

    Task<long> GenerateDepartmentCode(CancellationToken cancellationToken);

    Task<int> CreateDepartment(Department department, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchDepartment> GetDepartment(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchDepartment>> ListDepartments(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default);

    #endregion Department

    #region Teams

    Task<IEnumerable<FetchTeam>> ListTeams(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default);

    #endregion
}

public class ManageRepository : BasePostgresRepository, IManageRepository
{
    public ManageRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    #region Business Hour

    public Task<int> CreateBusinessHour(BusinessHour businessHour, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
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

    public Task<FetchBusinessHour> GetBusinessHour(string id, CancellationToken cancellationToken)
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

    #endregion Business Hour

    #region Tag

    public Task<int> CreateTag(Tag tag, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<FetchTag> GetTag(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<FetchTag>> ListTags(int offset, int limit, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion Tag

    #region Department

    public Task<long> GenerateDepartmentCode(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(conn =>
        {
            var query = "SELECT NEXTVAL('DepartmentCode');";

            return conn.QuerySingleAsync<long>(query);
        });
    }

    public Task<int> CreateDepartment(Department department, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var cmd = @"
                INSERT INTO public.department
                    (id
                    , code
                    , ""name""
                    , businesshour
                    , outgoingemail
                    , ""type""
                    , response
                    , status
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES(@Id
                    , @Code
                    , @Name
                    , @BusinessHour
                    , @OutgoingEmail
                    , @Type
                    , @Response
                    , @Status
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy);
                ";

            var crst = await conn.ExecuteAsync(cmd, new
            {
                department.Id,
                department.Code,
                department.Name,
                department.BusinessHour,
                department.OutgoingEmail,
                department.Type,
                department.Response,
                department.RowVersion,
                department.Status,
                department.CreatedBy,
                department.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.departmentauditlog
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

    public Task<FetchDepartment> GetDepartment(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT d.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = d.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = d.updatedby) AS UpdatedByName
                    , b.""name"" AS businesshourname
                    , CONCAT(a.firstname, ' ', a.lastname) as ManagerName
                FROM department d
                LEFT JOIN businesshour b ON b.id = d.businesshour 
                LEFT JOIN agent a on a.id = d.manager
                WHERE d.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchDepartment>(query, new { Id = id });
        });
    }

    public Task<IEnumerable<FetchDepartment>> ListDepartments(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND d.deleted = FALSE ";
            var query = $@"
                SELECT d.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = d.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = d.updatedby) AS UpdatedByName
                FROM department b
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchDepartment>(query, new { offset, limit });
        });
    }

    #endregion Department

    #region Team

    public
    Task<IEnumerable<FetchTeam>> ListTeams(int offset = 0, int limit = 1000, bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND t.deleted = FALSE ";
            var query = $@"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                FROM team t
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchTeam>(query, new { offset, limit });
        });
    }

    #endregion
}