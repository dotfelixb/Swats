using Dapper;
using Keis.Model;
using Keis.Model.Domain;
using Keis.Model.Queries;
using Microsoft.Extensions.Options;

namespace Keis.Data.Repository;

public interface IManageRepository
{
    #region Business Hour

    Task<int> CreateBusinessHour(BusinessHour businessHour, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchBusinessHour> GetBusinessHour(string id, CancellationToken cancellationToken);

    Task<IEnumerable<OpenHour>> GetBusinessHourOpens(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchBusinessHour>> ListBusinessHours(int offset = 0, int limit = 1000,
        bool includeDeleted = false, CancellationToken cancellationToken = default);

    #endregion Business Hour

    #region Tags

    Task<int> CreateTag(Tag tag, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchTag> GetTag(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchTag>> ListTags(int offset, int limit, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    #endregion Tags

    #region Department

    Task<long> GenerateDepartmentCode(CancellationToken cancellationToken);

    Task<int> CreateDepartment(Department department, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchDepartment> GetDepartment(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchDepartment>> ListDepartments(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    #endregion Department

    #region Teams

    Task<int> CreateTeam(Team team, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchTeam> GetTeam(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchTeam>> ListTeams(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    #endregion Teams

    #region HelpTopic

    Task<int> CreateHelpTopic(HelpTopic helpTopic, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchHelpTopic> GetHelpTopic(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchHelpTopic>> ListHelpTopics(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    #endregion HelpTopic

    #region Sla

    Task<int> CreateSla(Sla sla, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<FetchSla> GetSla(string id, CancellationToken cancellationToken);

    Task<IEnumerable<FetchSla>> ListSla(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    #endregion Sla

    #region Workflow

    Task<int> CreateWorkflow(Workflow workflow, DbAuditLog auditLog, CancellationToken cancellationToken);

    Task<IEnumerable<FetchWorkflow>> ListWorkflow(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default);

    Task<FetchWorkflow> GetWorkflow(string id, CancellationToken cancellationToken);

    #endregion
}

public class ManageRepository : BasePostgresRepository, IManageRepository
{
    public ManageRepository(IOptions<ConnectionStringOptions> options) : base(options)
    {
    }

    #region Business Hour

    public Task<int> CreateBusinessHour(BusinessHour businessHour, DbAuditLog auditLog,
        CancellationToken cancellationToken)
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

            var hoursCmd = @"INSERT INTO public.businessopenhour
                    (id
                    , businesshour
                    , ""name""
                    , enabled
                    , fullday
                    , fromtime
                    , totime)
                VALUES(@Id
                    , @BusinessHour
                    , @Name
                    , @Enabled
                    , @FullDay
                    , @FromTime
                    , @ToTime)";

            var hours = businessHour.OpenHours.Select(h => new
            {
                h.Id,
                BusinessHour = businessHour.Id,
                h.Name,
                h.Enabled,
                h.FullDay,
                h.FromTime,
                h.ToTime
            }).ToArray();
            var hcrst = await conn.ExecuteAsync(hoursCmd, hours);

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

            return await conn.QueryFirstOrDefaultAsync<FetchBusinessHour>(query, new {Id = id});
        });
    }

    public Task<IEnumerable<OpenHour>> GetBusinessHourOpens(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT bo.*
                FROM businessopenhour bo
                WHERE bo.businesshour = @Id";

            return await conn.QueryAsync<OpenHour>(query, new {Id = id});
        });
    }

    public Task<IEnumerable<FetchBusinessHour>> ListBusinessHours(int offset = 0, int limit = 1000,
        bool includeDeleted = false, CancellationToken cancellationToken = default)
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

            return await conn.QueryAsync<FetchBusinessHour>(query, new {offset, limit});
        });
    }

    #endregion Business Hour

    #region Tag

    public Task<int> CreateTag(Tag tag, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var cmd = @"
                    INSERT INTO public.tags
                        (id
                        , ""name""
                        , note
                        , color
                        , visibility
                        , status
                        , rowversion
                        , createdby
                        , updatedby)
                    VALUES(@Id
                        , @Name
                        , @Note
                        , @Color
                        , @Visibility
                        , @Status
                        , @RowVersion
                        , @CreatedBy
                        , @UpdatedBy);
                    ";

            var crst = await conn.ExecuteAsync(cmd, new
            {
                tag.Id,
                tag.Name,
                tag.Note,
                tag.Color,
                tag.Visibility,
                tag.Status,
                tag.RowVersion,
                tag.CreatedBy,
                tag.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.tagsauditlog
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

    public Task<FetchTag> GetTag(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                FROM tags t
                WHERE t.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchTag>(query, new {Id = id});
        });
    }

    public Task<IEnumerable<FetchTag>> ListTags(int offset, int limit, bool includeDeleted = false,
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
                FROM tags t
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchTag>(query, new {offset, limit});
        });
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
                    , CONCAT(g.firstname, ' ', g.lastname) as ManagerName
                FROM department d
                LEFT JOIN businesshour b ON b.id = d.businesshour
                LEFT JOIN agent g on g.id = d.manager
                WHERE d.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchDepartment>(query, new {Id = id});
        });
    }

    public Task<IEnumerable<FetchDepartment>> ListDepartments(int offset = 0, int limit = 1000,
        bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND d.deleted = FALSE ";
            var query = $@"
                SELECT d.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = d.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = d.updatedby) AS UpdatedByName
                    , b.""name"" AS businesshourname
                    , CONCAT(g.firstname, ' ', g.lastname) as ManagerName
                FROM department d
                LEFT JOIN businesshour b ON b.id = d.businesshour
                LEFT JOIN agent g on g.id = d.manager
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchDepartment>(query, new {offset, limit});
        });
    }

    #endregion Department

    #region Team

    public Task<int> CreateTeam(Team team, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var cmd = @"
                INSERT INTO public.team
                    (id
                    , ""name""
                    , department
                    , ""lead""
                    , status
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES(@Id
                    , @Name
                    , @Department
                    , @Lead
                    , @Status
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy);
                ";

            var crst = await conn.ExecuteAsync(cmd, new
            {
                team.Id,
                team.Name,
                team.Department,
                team.Lead,
                team.Status,
                team.RowVersion,
                team.CreatedBy,
                team.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.teamauditlog
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

    public Task<FetchTeam> GetTeam(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT t.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = t.updatedby) AS UpdatedByName
                    , d.""name"" AS departmentname
                    , CONCAT(g.firstname, ' ', g.lastname) as LeadName
                FROM team t
                LEFT JOIN department d ON d.id = t.department
                LEFT JOIN agent g on g.id = d.manager
                WHERE t.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchTeam>(query, new {Id = id});
        });
    }

    public Task<IEnumerable<FetchTeam>> ListTeams(int offset = 0, int limit = 1000, bool includeDeleted = false,
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
                    , CONCAT(g.firstname, ' ', g.lastname) as LeadName
                FROM team t
                LEFT JOIN department d ON d.id = t.department
                LEFT JOIN agent g on g.id = d.manager
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchTeam>(query, new {offset, limit});
        });
    }

    #endregion Team

    #region HelpTopic

    public Task<int> CreateHelpTopic(HelpTopic helpTopic, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var cmd = @"
                INSERT INTO public.helptopic
                    (id
                    , ""name""
                    , ""type""
                    , department
                    , note
                    , status
                    , rowversion
                    , createdby
                    , updatedby)
                VALUES(@Id
                    , @Name
                    , @Type
                    , @Department
                    , @Note
                    , @Status
                    , @RowVersion
                    , @CreatedBy
                    , @UpdatedBy);
                ";

            var crst = await conn.ExecuteAsync(cmd, new
            {
                helpTopic.Id,
                helpTopic.Name,
                helpTopic.Type,
                helpTopic.Department,
                helpTopic.Note,
                helpTopic.Status,
                helpTopic.RowVersion,
                helpTopic.CreatedBy,
                helpTopic.UpdatedBy
            });

            var logCmd = @"
                INSERT INTO public.helptopicauditlog
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

    public Task<FetchHelpTopic> GetHelpTopic(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT h.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = h.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = h.updatedby) AS UpdatedByName
                    , d.""name"" AS departmentname
                FROM helptopic h
                LEFT JOIN department d ON d.id = h.department
                WHERE h.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchHelpTopic>(query, new {Id = id});
        });
    }

    public Task<IEnumerable<FetchHelpTopic>> ListHelpTopics(int offset = 0, int limit = 1000,
        bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND h.deleted = FALSE ";
            var query = $@"
                SELECT h.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = h.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = h.updatedby) AS UpdatedByName
                    , d.""name"" AS departmentname
                FROM helptopic h
                LEFT JOIN department d ON d.id = h.department
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchHelpTopic>(query, new {offset, limit});
        });
    }

    #endregion HelpTopic

    #region Sla

    public Task<int> CreateSla(Sla sla, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            const string cmd = @"
                    INSERT INTO public.sla
                        (id
                        , ""name""
                        , businesshour
                        , responseperiod
                        , responseformat
                        , responsenotify
                        , responseemail
                        , resolveperiod
                        , resolveformat
                        , resolvenotify
                        , resolveemail
                        , note
                        , status
                        , rowversion
                        , createdby
                        , updatedby)
                    VALUES(@Id
                        , @Name
                        , @BusinessHour
                        , @ResponsePeriod
                        , @ResponseFormat
                        , @ResponseNotify
                        , @ResponseEmail
                        , @ResolvePeriod
                        , @ResolveFormat
                        , @ResolveNotify
                        , @ResolveEmail
                        , @Note
                        , @Status
                        , @RowVersion
                        , @CreatedBy
                        , @UpdatedBy)";

            var ctt = await conn.ExecuteAsync(cmd, new
            {
                sla.Id,
                sla.Name,
                sla.BusinessHour,
                sla.ResponsePeriod,
                sla.ResponseFormat,
                sla.ResponseNotify,
                sla.ResponseEmail,
                sla.ResolvePeriod,
                sla.ResolveFormat,
                sla.ResolveNotify,
                sla.ResolveEmail,
                sla.Note,
                sla.Status,
                sla.RowVersion,
                sla.CreatedBy,
                sla.UpdatedBy
            });

            const string logCmd = @"
                INSERT INTO public.slaauditlog
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

    public Task<IEnumerable<FetchSla>> ListSla(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND s.deleted = FALSE ";
            var query = $@"
                SELECT s.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = s.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = s.updatedby) AS UpdatedByName
                    , b.""name"" AS businesshourname
                FROM sla s
                LEFT JOIN businesshour b ON b.id = s.businesshour
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchSla>(query, new {offset, limit});
        });
    }

    public Task<FetchSla> GetSla(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var query = @"
                SELECT s.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = s.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = s.updatedby) AS UpdatedByName
                    , b.""name"" AS businesshourname
                FROM sla s
                LEFT JOIN businesshour b ON b.id = s.businesshour
                WHERE s.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchSla>(query, new {Id = id});
        });
    }

    #endregion Sla

    #region Workflow

    public Task<int> CreateWorkflow(Workflow workflow, DbAuditLog auditLog, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            const string cmd = @"
                    INSERT INTO public.workflow
                        (id
                        , ""name""
                        , events
                        , note
                        , priority
                        , status
                        , rowversion
                        , createdby
                        , updatedby)
                    VALUES(@Id
                        , @Name
                        , @Events
                        , @Note
                        , @Priority
                        , @Status
                        , @RowVersion
                        , @CreatedBy
                        , @UpdatedBy)";

            var eventArray = workflow.Events.Select(s => (int) s).ToArray();
            var cmdRst = await conn.ExecuteAsync(cmd, new
            {
                workflow.Id,
                workflow.Name,
                Events = eventArray,
                workflow.Priority,
                workflow.Note,
                workflow.Status,
                workflow.RowVersion,
                workflow.CreatedBy,
                workflow.UpdatedBy
            });

            const string cmdCrt = @"
                    INSERT INTO public.workflowcriteria
                        (id
                        , workflow
                        , ""name""
                        , criteria
                        , ""condition""
                        , ""match"")
                    VALUES(@Id
                        , @Workflow
                        , @Name
                        , @Criteria
                        , @Condition
                        , @Match);";


            var criteria = workflow.Criteria.Select(s => new
            {
                s.Id,
                Workflow = workflow.Id,
                s.Name,
                Criteria = (int) s.Criteria,
                Condition = (int) s.Condition,
                s.Match
            }).ToArray();
            var crtRst = await conn.ExecuteAsync(cmdCrt, criteria);

            const string cmdAct = @"
                    INSERT INTO public.workflowaction
                        (id
                        , workflow
                        , ""name""
                        , actionfrom
                        , actionto)
                    VALUES(@Id
                        , @Workflow
                        , @Name
                        , @ActionFrom
                        , @ActionTo);
                        ";

            var actions = workflow.Actions.Select(s => new
            {
                s.Id,
                Workflow = workflow.Id,
                s.Name,
                ActionFrom = (int) s.ActionFrom,
                s.ActionTo
            }).ToArray();
            var actRst = await conn.ExecuteAsync(cmdAct, actions);

            const string logCmd = @"
                INSERT INTO public.workflowauditlog
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

            var logRst = await conn.ExecuteAsync(logCmd, new
            {
                auditLog.Id,
                auditLog.Target,
                auditLog.ActionName,
                auditLog.Description,
                auditLog.ObjectName,
                auditLog.ObjectData,
                auditLog.CreatedBy
            });

            return cmdRst + crtRst + actRst + logRst;
        });
    }

    public Task<IEnumerable<FetchWorkflow>> ListWorkflow(int offset = 0, int limit = 1000, bool includeDeleted = false,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            var _includeDeleted = includeDeleted ? " " : " AND w.deleted = FALSE ";
            var query = $@"
                SELECT w.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = w.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = w.updatedby) AS UpdatedByName
                FROM workflow w
                WHERE 1=1
                {_includeDeleted}
                OFFSET @Offset LIMIT @Limit;
                ";

            return await conn.QueryAsync<FetchWorkflow>(query, new {offset, limit});
        });
    }

    public Task<FetchWorkflow> GetWorkflow(string id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return WithConnection(async conn =>
        {
            const string query = @"
                SELECT w.*
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = w.createdby) AS CreatedByName
	                , (SELECT a.normalizedusername FROM authuser a WHERE a.id = w.updatedby) AS UpdatedByName
                FROM workflow w
                WHERE w.id = @Id";

            return await conn.QueryFirstOrDefaultAsync<FetchWorkflow>(query, new {Id = id});
        });
    }

    #endregion
}