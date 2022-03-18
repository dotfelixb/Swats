using FluentResults;
using MediatR;

namespace Swats.Model.Commands;

public class CreateTagCommand : IRequest<Result>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
}
