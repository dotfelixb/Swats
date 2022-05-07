using Keis.Infrastructure.Features.Sla.CreateSla;
using Keis.Infrastructure.Features.Sla.GetSla;
using Keis.Infrastructure.Features.Sla.ListSla;
using Keis.Model;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class SlaController : MethodController
{
    private readonly ILogger<SlaController> logger;
    private readonly IMediator mediatr;

    public SlaController(ILogger<SlaController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("sla.list", Name = nameof(ListSlas))]
    public async Task<IActionResult> ListSlas([FromQuery] ListSlaCommand command)
    {
        const string msg = $"GET::{nameof(SlaController)}::{nameof(ListSlas)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchSla>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("sla.get", Name = nameof(GetSla))]
    public async Task<IActionResult> GetSla([FromQuery] GetSlaCommand command)
    {
        const string msg = $"GET::{nameof(SlaController)}::{nameof(GetSla)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchSla>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("sla.create", Name = nameof(CreateSla))]
    public async Task<IActionResult> CreateSla(CreateSlaCommand command)
    {
        const string msg = $"POST::{nameof(SlaController)}::{nameof(CreateSla)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/sla.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}