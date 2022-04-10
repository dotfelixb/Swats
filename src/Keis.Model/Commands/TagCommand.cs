using FluentResults;
using MediatR;
using Keis.Model.Domain;
using Keis.Model.Queries;

namespace Keis.Model.Commands;

public class CreateTagCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
    public DefaultStatus Status { get; set; }
    public string CreatedBy { get; set; }
}

public class GetTagCommand : GetType, IRequest<Result<FetchTag>>
{
}

public class ListTagsCommand : ListType, IRequest<Result<IEnumerable<FetchTag>>>
{
}