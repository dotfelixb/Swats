using Keis.Infrastructure.Features.Template.CreateTemplate;
using Keis.Infrastructure.Features.Template.GetTemplate;
using Keis.Infrastructure.Features.Template.ListTemplates;
using Keis.Infrastructure.Features.Template.ListTemplateTags;
using Keis.Infrastructure.Features.Template.UpdateTemplate;
using Keis.Model;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class TemplateController : MethodController
{
    private readonly ILogger<TagController> logger;
    private readonly IMediator mediatr;

    public TemplateController(ILogger<TagController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("template.list", Name = nameof(ListTemplates))]
    public async Task<IActionResult> ListTemplates([FromQuery] ListTemplatesCommand command)
    {
        const string msg = $"GET::{nameof(TemplateController)}::{nameof(ListTemplates)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchTemplate>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("template.get", Name = nameof(GetTemplate))]
    public async Task<IActionResult> GetTemplate([FromQuery] GetTemplateCommand command)
    {
        const string msg = $"GET::{nameof(TemplateController)}::{nameof(GetTemplate)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchTemplate>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("template.mergetags", Name = nameof(ListTemplateTags))]
    public async Task<IActionResult> ListTemplateTags([FromQuery] ListTemplateTagsCommand command)
    {
        const string msg = $"GET::{nameof(TemplateController)}::{nameof(ListTemplateTags)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("template.create", Name = nameof(CreateTemplate))]
    public async Task<IActionResult> CreateTemplate(CreateTemplateCommand command)
    {
        const string msg = $"POST::{nameof(TemplateController)}::{nameof(CreateTemplate)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/template.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("template.update", Name = nameof(UpdateTemplate))]
    public async Task<IActionResult> UpdateTemplate(UpdateTemplateCommand command)
    {
        const string msg = $"PATCH::{nameof(TemplateController)}::{nameof(UpdateTemplate)}";
        logger.LogInformation(msg);

        command.UpdatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/template.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}
