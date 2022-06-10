using FluentResults;
using MediatR;

namespace Keis.Infrastructure.Features.Template.CreateTemplate;

public class CreateTemplateCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string[] MergeTags { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string Status { get; set; }
    public string CreatedBy { get; set; }
}
