using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Template.ListTemplates
{
    public class ListTemplatesCommand : ListType, IRequest<Result<IEnumerable<FetchTemplate>>>
    {
    }
}
