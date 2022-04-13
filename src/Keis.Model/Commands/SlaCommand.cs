using FluentResults;
using MediatR;
using Keis.Model.Domain;
using Keis.Model.Queries;

namespace Keis.Model.Commands;

public class CreateSlaCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string BusinessHour { get; set; }
    public int ResponsePeriod { get; set; }
    public DefaultTimeFormat ResponseFormat { get; set; }
    public bool ResponseNotify { get; set; }
    public bool ResponseEmail { get; set; }
    public int ResolvePeriod { get; set; }
    public DefaultTimeFormat ResolveFormat { get; set; }
    public bool ResolveNotify { get; set; }
    public bool ResolveEmail { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

public class GetSlaCommand : GetType, IRequest<Result<FetchSla>>
{
}

public class ListSlaCommand : ListType, IRequest<Result<IEnumerable<FetchSla>>>
{
}