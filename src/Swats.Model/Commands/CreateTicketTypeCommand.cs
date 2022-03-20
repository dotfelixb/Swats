using FluentResults;
using MediatR;
using Swats.Model.Domain;

namespace Swats.Model.Commands;

public class CreateTicketTypeCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Color { get; set; }
    public DefaultType Visibility { get; set; }
    public string CreatedBy { get; set; }
}
