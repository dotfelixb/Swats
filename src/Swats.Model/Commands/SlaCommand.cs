using FluentResults;
using MediatR;
using Swats.Model.Queries;

namespace Swats.Model.Commands;

public class CreateSlaCommand : IRequest<Result<string>>
{
    
}

public class GetSlaCommand : GetType, IRequest<Result<FetchSla>>
{
}

public class ListSlasCommand : ListType, IRequest<Result<IEnumerable<FetchSla>>>
{
}