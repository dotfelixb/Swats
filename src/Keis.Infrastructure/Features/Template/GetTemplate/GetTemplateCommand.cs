using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Template.GetTemplate
{
    public class GetTemplateCommand : GetType, IRequest<Result<FetchTemplate>>
    {
    }
}
