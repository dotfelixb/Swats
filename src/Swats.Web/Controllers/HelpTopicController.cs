using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Model.Queries;
using Swats.Web.Extensions;

namespace Swats.Web.Controllers;

public class HelpTopicController : MethodController
{
    private readonly ILogger<HelpTopicController> logger;
    private readonly IMediator mediatr;

    public HelpTopicController(ILogger<HelpTopicController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("helptopic.list", Name = nameof(ListTopic))]
    public async Task<IActionResult> ListTopic([FromQuery] ListHelpTopicsCommand command)
    {
        const string msg = $"GET::{nameof(HelpTopicController)}::{nameof(ListTopic)}";
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

        return Ok(new ListResult<FetchHelpTopic>
        {
            Ok = true,
            Data = result.Value
        });
    }
    
    [HttpGet("helptopic.get", Name = nameof(GetTopic))]
    public async Task<IActionResult> GetTopic([FromQuery] GetHelpTopicCommand command)
    {
        const string msg = $"GET::{nameof(HelpTopicController)}::{nameof(GetTopic)}";
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

        return Ok(new SingleResult<FetchHelpTopic>
        {
            Ok = true,
            Data = result.Value
        });
    }
    
    [HttpPost("helptopic.create", Name = nameof(CreateTopic))]
    public async Task<IActionResult> CreateTopic(CreateHelpTopicCommand command)
    {
        const string msg = $"POST::{nameof(BusinessHourController)}::{nameof(CreateTopic)}";
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
        
        var uri = $"/methods/helptopic.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}