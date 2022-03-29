using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swats.Model;
using Swats.Model.Commands;

namespace Swats.Web.Controllers;

public class AgentController : MethodController
{
    private readonly ILogger<AgentController> logger;
    private readonly IMediator mediatr;

    public AgentController(ILogger<AgentController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("agents.list", Name = nameof(ListTickets))]
    public async Task<IActionResult> ListTickets([FromQuery] ListTicketCommand command)
    {
        logger.LogInformation($"{Request.Method}::{nameof(AgentController)}::{nameof(ListTickets)}");

        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            return NotFound(new ErrorResult { Ok = false });
        }
        return Ok(result.Value);
    }

    [HttpGet("agents.get", Name = nameof(GetAgent))]
    public Task<IActionResult> GetAgent([FromQuery] GetAgentCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPost("agents.create", Name = nameof(CreateAgent))]
    public Task<IActionResult> CreateAgent(CreateAgentCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("agents.department", Name = nameof(AssignAgentDepartment))]
    public Task<IActionResult> AssignAgentDepartment(AssignAgentDepartmentCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("agents.team", Name = nameof(AssignAgentTeam))]
    public Task<IActionResult> AssignAgentTeam(AssignAgentTeamCommand command)
    {
        throw new NotImplementedException();
    }
}