using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Template.ListTemplateTags;

internal class ListTemplateTagsCommandHandler : IRequestHandler<ListTemplateTagsCommand, Result<IEnumerable<string>>>
{
    public Task<Result<IEnumerable<string>>> Handle(ListTemplateTagsCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<string> tags = new List<string>
        {
            "{email}",
            "{first_name}",
            "{last_name}",
        };

        return Task.FromResult(Result.Ok(tags));
    }
}
