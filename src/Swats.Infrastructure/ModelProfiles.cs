using Swats.Model.Commands;
using Swats.Model.Domain;

namespace Swats.Infrastructure;

public class ModelProfiles : AutoMapper.Profile
{
    public ModelProfiles()
    {
        CreateMap<CreateTicketCommand, Ticket>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy))
            .ForMember(d => d.RowVersion, opt => Guid.NewGuid());
       
        CreateMap<CreateTicketTypeCommand, TicketType>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s=> s.CreatedBy))
            .ForMember(d => d.RowVersion, opt => Guid.NewGuid());
       
        CreateMap<CreateBusinessHourCommand, BusinessHour>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy))
            .ForMember(d => d.RowVersion, opt => Guid.NewGuid());
       
        CreateMap<CreateAgentCommand, Agent>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy))
            .ForMember(d => d.RowVersion, opt => Guid.NewGuid());

        CreateMap<CreateAgentCommand, AuthUser>()
            .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Mobile))
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy))
            .ForMember(d => d.SecurityStamp, opt => Guid.NewGuid())
            .ForMember(d => d.RowVersion, opt => Guid.NewGuid());

        /**
         *.ForMember(d => d.ParentCustomer,
         *          opt => opt.MapFrom(s => s.ParentCustomer.ToGuid()))
         */
    }
}
