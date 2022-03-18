using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Queries;

public class BusinessHourQueries
{
}

public class FetchBusinessHour : BusinessHour
{
    public string ImageCode { get; set; }
    public string CreatedByName { get; set; }
    public string UpdatedByName { get; set; }
}

public class ListBusinessHourCommand : ListType, IRequest<Result<IEnumerable<FetchBusinessHour>>>
{

}