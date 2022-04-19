using FluentResults;
using Keis.Model.Domain;
using MediatR;

namespace Keis.Infrastructure.Features.BusinessHour.CreateBusinessHour;

public class CreateBusinessHourCommand : IRequest<Result<string>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Timezone { get; set; }
    public DefaultStatus Status { get; set; }
    public OpenHour[] OpenHours { get; set; } = Array.Empty<OpenHour>();
    public string CreatedBy { get; set; }
}