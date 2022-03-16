using Swats.Model.Commands;
using Swats.Model.Domain;

namespace Swats.Infrastructure;

public class ModelProfiles : AutoMapper.Profile
{
    public ModelProfiles()
    {
        CreateMap<CreateTicketCommand, Ticket>();
        CreateMap<CreateTicketTypeCommand, TicketType>();
    }
}
