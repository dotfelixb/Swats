using FluentResults;
using MediatR;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class CreateTagCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public string CreatedBy { get; set; }
}

public class GetTagCommand : GetType, IRequest<Result<FetchTag>>
{

}

public class ListTagsCommand : ListType, IRequest<Result<IEnumerable<FetchTag>>>
{

}
