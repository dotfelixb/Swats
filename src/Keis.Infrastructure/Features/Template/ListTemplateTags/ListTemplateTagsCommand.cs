using FluentResults;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Template.ListTemplateTags;

public class ListTemplateTagsCommand : ListType, IRequest<Result<IEnumerable<string>>>
{
}
