using Keis.Infrastructure.Features.Workflow.CreateWorkflow;
using Keis.Infrastructure.Features.Workflow.GetWorkflow;
using Keis.Infrastructure.Features.Workflow.ListWorkflow;
using Keis.Infrastructure.Features.Workflow.ListWorkflowAction;
using Keis.Infrastructure.Features.Workflow.ListWorkflowCriteria;
using Keis.Infrastructure.Features.Workflow.ListWorkflowEvent;
using Keis.Model;
using Keis.Model.Domain;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Keis.Web.Controllers;

public class WorkflowController : MethodController
{
    private readonly ILogger<WorkflowController> _logger;
    private readonly IMediator _mediatr;

    public WorkflowController(ILogger<WorkflowController> logger, IMediator mediatr)
    {
        _logger = logger;
        _mediatr = mediatr;
    }

    [HttpGet("workflow.list", Name = nameof(ListWorkflows))]
    public async Task<IActionResult> ListWorkflows([FromQuery] ListWorkflowCommand command)
    {
        const string msg = $"GET::{nameof(WorkflowController)}::{nameof(ListWorkflows)}";
        _logger.LogInformation(msg);

        var result = await _mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchWorkflow>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("workflow.get", Name = nameof(GetWorkflow))]
    public async Task<IActionResult> GetWorkflow([FromQuery] GetWorkflowCommand command)
    {
        const string msg = $"GET::{nameof(WorkflowController)}::{nameof(GetWorkflow)}";
        _logger.LogInformation(msg);

        var result = await _mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchWorkflow>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("workflow.event", Name = nameof(ListWorkflowEvent))]
    public async Task<IActionResult> ListWorkflowEvent([FromQuery] ListWorkflowEventCommand command)
    {
        const string msg = $"GET::{nameof(WorkflowController)}::{nameof(ListWorkflowEvent)}";
        _logger.LogInformation(msg);

        var result = await _mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<WorkflowEvent>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("workflow.criteria", Name = nameof(ListWorkflowCriteria))]
    public async Task<IActionResult> ListWorkflowCriteria([FromQuery] ListWorkflowCriteriaCommand command)
    {
        const string msg = $"GET::{nameof(WorkflowController)}::{nameof(ListWorkflowCriteria)}";
        _logger.LogInformation(msg);

        var result = await _mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<WorkflowCriteria>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("workflow.action", Name = nameof(ListWorkflowAction))]
    public async Task<IActionResult> ListWorkflowAction([FromQuery] ListWorkflowActionCommand command)
    {
        const string msg = $"GET::{nameof(WorkflowController)}::{nameof(ListWorkflowAction)}";
        _logger.LogInformation(msg);

        var result = await _mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<WorkflowAction>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("workflow.create", Name = nameof(CreateWorkflow))]
    public async Task<IActionResult> CreateWorkflow(CreateWorkflowCommand command)
    {
        const string msg = $"POST::{nameof(WorkflowController)}::{nameof(CreateWorkflow)}";
        _logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await _mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/workflow.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}