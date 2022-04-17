using AutoMapper;
using FluentResults;
using MediatR;
using Keis.Data.Repository;
using Keis.Infrastructure.Extensions;
using Keis.Model.Commands;
using Keis.Model.Domain;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Keis.Infrastructure.Features.Tickets.CreateTicket;

public class CreateTicketCommand : IRequest<Result<string>>
{
    public string Subject { get; set; }
    public string Requester { get; set; }
    public string RequesterEmail { get; set; }
    public string RequesterName { get; set; }
    public IEnumerable<SelectListItem> RequesterList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Body { get; set; }
    public string AssignedTo { get; set; }
    public IEnumerable<SelectListItem> AssigneeList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string ExternalAgent { get; set; } // user by api to identify the keis user that created this ticket
    public TicketSource Source { get; set; }
    public string TicketType { get; set; }
    public IEnumerable<SelectListItem> TypeList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Department { get; set; }
    public IEnumerable<SelectListItem> DepartmentList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string Team { get; set; }
    public IEnumerable<SelectListItem> TeamList { get; set; } = Enumerable.Empty<SelectListItem>();
    public string HelpTopic { get; set; }
    public IEnumerable<SelectListItem> HelpTopicList { get; set; } = Enumerable.Empty<SelectListItem>();
    public TicketPriority Priority { get; set; } = TicketPriority.Low;
    public TicketStatus Status { get; set; } = TicketStatus.New;
    public string CreatedBy { get; set; }
}

public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Result<string>>
{
    private readonly IMapper _mapper;
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketCommandHandler(ITicketRepository ticketRepository, IMapper mapper)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
    }

    public async Task<Result<string>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = _mapper.Map<CreateTicketCommand, Ticket>(request);
        ticket.UpdatedBy = request.CreatedBy;

        var code = await _ticketRepository.GenerateTicketCode(cancellationToken);
        ticket.Code = code.FormatCode("#PT-"); // TODO : Get Prefix from appsettings

        var comment = new TicketComment
        {
            Ticket = ticket.Id,
            FromEmail = request.RequesterEmail,
            FromName = request.RequesterName,
            Body = request.Body,
            Type = CommentType.Comment,
            Source = TicketSource.App,
            CreatedBy = request.CreatedBy
        };

        var auditLog = new DbAuditLog
        {
            Target = ticket.Id,
            ActionName = "ticket.create",
            Description = "added ticket",
            ObjectName = "ticket",
            ObjectData = JsonSerializer.Serialize(ticket),
            CreatedBy = request.CreatedBy
        };

        var rst = await _ticketRepository.CreateTicket(ticket, comment, auditLog, cancellationToken);
        
        // use db transaction
        return rst > 2 ? Result.Ok(ticket.Id) : Result.Fail<string>("Not able to create now!");
    }
}