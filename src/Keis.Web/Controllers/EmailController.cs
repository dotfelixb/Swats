using Keis.Infrastructure.Features.Emails.CreateEmail;
using Keis.Infrastructure.Features.Emails.GetEmail;
using Keis.Infrastructure.Features.Emails.ListEmails;
using Keis.Infrastructure.Features.Emails.UpdateEmail;
using Keis.Model;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class EmailController : MethodController {

    private readonly ILogger<EmailController> logger;
    private readonly IMediator mediatr;

    public EmailController(ILogger<EmailController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("email.list", Name = nameof(ListEmail))]
    public async Task<IActionResult> ListEmail([FromQuery] ListEmailsCommand command)
    {
        const string msg = $"GET::{nameof(EmailController)}::{nameof(ListEmail)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchEmail>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("email.get", Name = nameof(GetEmail))]
    public async Task<IActionResult> GetEmail([FromQuery] GetEmailCommand command)
    {
        const string msg = $"GET::{nameof(EmailController)}::{nameof(GetEmail)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchEmail>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("email.create", Name = nameof(CreateEmail))]
    public async Task<IActionResult> CreateEmail(CreateEmailCommand command)
    {
        const string msg = $"POST::{nameof(EmailController)}::{nameof(CreateEmail)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/email.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("email.update", Name = nameof(UpdateEmail))]
    public async Task<IActionResult> UpdateEmail(UpdateEmailCommand command)
    {
        const string msg = $"PATCH::{nameof(EmailController)}::{nameof(UpdateEmail)}";
        logger.LogInformation(msg);

        command.UpdatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/email.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}