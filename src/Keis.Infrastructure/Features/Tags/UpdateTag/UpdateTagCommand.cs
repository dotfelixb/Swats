using FluentResults;
using Keis.Model.Domain;
using Keis.Model.Queries;
using MediatR;

namespace Keis.Infrastructure.Features.Tags.UpdateTag;

public class UpdateTagCommand : IRequest<Result<string>>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
    public DefaultStatus Status { get; set; }
    public string UpdatedBy { get; set; }
}
