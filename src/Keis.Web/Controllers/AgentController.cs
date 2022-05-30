using AutoMapper;
using Keis.Infrastructure.Features.Agents.CreateAgent;
using Keis.Infrastructure.Features.Agents.GetAgent;
using Keis.Infrastructure.Features.Agents.ListAgent;
using Keis.Infrastructure.Features.Agents.UpdateAgent;
using Keis.Model;
using Keis.Model.Commands;
using Keis.Model.Domain;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class AgentController : MethodController
{
    private readonly ILogger<AgentController> logger;
    private readonly IMapper mapper;
    private readonly IMediator mediatr;
    private readonly UserManager<AuthUser> userManager;

    public AgentController(ILogger<AgentController> logger
        , IMapper mapper
        , IMediator mediatr
        , UserManager<AuthUser> userManager)
    {
        this.logger = logger;
        this.mapper = mapper;
        this.mediatr = mediatr;
        this.userManager = userManager;
    }

    [HttpGet("agent.list", Name = nameof(ListAgents))]
    public async Task<IActionResult> ListAgents([FromQuery] ListAgentCommand command)
    {
        const string msg = $"GET::{nameof(AgentController)}::{nameof(ListAgents)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

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
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

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
        // get user name from email
        command.UserName = command.Email.Split('@').FirstOrDefault(); // lisa.paige@keis.app => lisa.paige

        // create a user
        logger.LogInformation($"{msg} - Creating user for agent");

        // email before at '@' symbol and @1
        var password = $"{command.UserName}@1"; // lisa.paige@keis.app => lisa.paige@1
        var user = mapper.Map<CreateAgentCommand, AuthUser>(command);
        var userResult = await userManager.CreateAsync(user, password);
        if (!userResult.Succeeded)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = userResult.Errors.Select(s => s.Description)
            });

        // create agent
        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/agent.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("agent.update", Name = nameof(UpdateAgent))]
    public async Task<IActionResult> UpdateAgent(UpdateAgentCommand command)
    {
        const string msg = $"PATCH::{nameof(AgentController)}::{nameof(UpdateAgent)}";
        logger.LogInformation(msg);

        command.UpdatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);
        
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

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