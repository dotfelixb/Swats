using Keis.Infrastructure.Features.Teams.CreateTeam;
using Keis.Infrastructure.Features.Teams.GetTeam;
using Keis.Infrastructure.Features.Teams.ListTeams;
using Keis.Model;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class TeamController : MethodController
{
    private readonly ILogger<TeamController> logger;
    private readonly IMediator mediatr;

    public TeamController(ILogger<TeamController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("team.list", Name = nameof(ListTeam))]
    public async Task<IActionResult> ListTeam([FromQuery] ListTeamsCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TeamController)}::{nameof(ListTeam)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchTeam>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("team.get", Name = nameof(GetTeam))]
    public async Task<IActionResult> GetTeam([FromQuery] GetTeamCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TeamController)}::{nameof(GetTeam)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchTeam>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("team.create", Name = nameof(CreateTeam))]
    public async Task<IActionResult> CreateTeam(CreateTeamCommand command)
    {
        var msg = $"{Request.Method}::{nameof(TeamController)}::{nameof(CreateTeam)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/team.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}