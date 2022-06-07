using AutoMapper;
using Keis.Infrastructure.Features.Agents.CreateAgent;
using Keis.Infrastructure.Features.Agents.UpdateAgent;
using Keis.Infrastructure.Features.BusinessHour.CreateBusinessHour;
using Keis.Infrastructure.Features.Department.CreateDepartment;
using Keis.Infrastructure.Features.Department.UpdateDepartment;
using Keis.Infrastructure.Features.Emails.CreateEmail;
using Keis.Infrastructure.Features.Emails.UpdateEmail;
using Keis.Infrastructure.Features.HelpTopic.CreateHelpTopic;
using Keis.Infrastructure.Features.Sla.CreateSla;
using Keis.Infrastructure.Features.Sla.UpdateSla;
using Keis.Infrastructure.Features.Tags.CreateTag;
using Keis.Infrastructure.Features.Tags.UpdateTag;
using Keis.Infrastructure.Features.Teams.CreateTeam;
using Keis.Infrastructure.Features.Teams.UpdateTeam;
using Keis.Infrastructure.Features.Tickets.CreateTicket;
using Keis.Infrastructure.Features.TicketTypes.CreateTicketType;
using Keis.Infrastructure.Features.Workflow.CreateWorkflow;
using Keis.Model.Commands;
using Keis.Model.Domain;

namespace Keis.Infrastructure;

public class ModelProfiles : Profile
{
    public ModelProfiles()
    {
        CreateMap<CreateTicketCommand, Ticket>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateTicketTypeCommand, TicketType>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateBusinessHourCommand, BusinessHour>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateAgentCommand, Agent>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateAgentCommand, AuthUser>()
            .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.Mobile))
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateDepartmentCommand, Department>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateTeamCommand, Team>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateHelpTopicCommand, HelpTopic>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateTagCommand, Tag>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateSlaCommand, Sla>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateWorkflowCommand, Workflow>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<CreateEmailCommand, Email>()
            .ForMember(d => d.UpdatedBy, opt => opt.MapFrom(s => s.CreatedBy));

        CreateMap<UpdateTagCommand, Tag>();
        CreateMap<UpdateAgentCommand, Agent>();
        CreateMap<UpdateDepartmentCommand, Department>();
        CreateMap<UpdateTeamCommand, Team>();
        CreateMap<UpdateSlaCommand, Sla>();
        CreateMap<UpdateEmailCommand, Email>();


        CreateMap<LoginLogCommand, LoginAudit>();
    }
}