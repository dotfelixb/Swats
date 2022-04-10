using MediatR;
using Microsoft.AspNetCore.Mvc;
using Keis.Model;
using Keis.Model.Commands;
using Keis.Model.Queries;
using Keis.Web.Extensions;

namespace Keis.Web.Controllers;

public class AgentController : MethodController
{
    private readonly ILogger<AgentController> logger;
    private readonly IMediator mediatr;

    public AgentController(ILogger<AgentController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("agent.list", Name = nameof(ListAgents))]
    public async Task<IActionResult> ListAgents([FromQuery] ListAgentCommand command)
    {
        const string msg = $"GET::{nameof(AgentController)}::{nameof(ListAgents)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });
        }

        return Ok(new ListResult<FetchAgent>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("agent.get", Name = nameof(GetAgent))]
    public async Task<IActionResult> GetAgent([FromQuery] GetAgentCommand command)
    {
        const string msg = $"GET::{nameof(AgentController)}::{nameof(GetAgent)}";
        logger.LogInformation(msg);
        
        var result = await mediatr.Send(command);
        if (result.IsFailed)
        {
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });
        }

        return Ok(new SingleResult<FetchAgent>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("agent.create", Name = nameof(CreateAgent))]
    public async Task<IActionResult> CreateAgent(CreateAgentCommand command)
    {
        const string msg = $"POST::{nameof(AgentController)}::{nameof(CreateAgent)}";
        logger.LogInformation(msg);
        
        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
        {
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });
        }
        
        var uri = $"/methods/agent.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("agent.department", Name = nameof(AssignAgentDepartment))]
    public Task<IActionResult> AssignAgentDepartment(AssignAgentDepartmentCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("agent.team", Name = nameof(AssignAgentTeam))]
    public Task<IActionResult> AssignAgentTeam(AssignAgentTeamCommand command)
    {
        throw new NotImplementedException();
    }
}