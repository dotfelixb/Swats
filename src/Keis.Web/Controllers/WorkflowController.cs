using Keis.Infrastructure.Features.Workflow.CreateWorkflow;
using Keis.Infrastructure.Features.Workflow.GetWorkflow;
using Keis.Infrastructure.Features.Workflow.ListWorkflow;
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
    private readonly ILogger<WorkflowController> logger;
    private readonly IMediator mediatr;

    public WorkflowController(ILogger<WorkflowController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("workflow.list", Name = nameof(ListWorkflows))]
    public async Task<IActionResult> ListWorkflows([FromQuery] ListWorkflowCommand command)
    {
        const string msg = $"GET::{nameof(WorkflowController)}::{nameof(ListWorkflows)}";
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

        return Ok(new SingleResult<FetchWorkflow>
        {
            Ok = true,
            Data = result.Value
        });
    }
    
    [HttpPost("workflow.create", Name = nameof(CreateWorkflow))]
    public async Task<IActionResult> CreateWorkflow(CreateWorkflowCommand command)
    {
        const string msg = $"POST::{nameof(WorkflowController)}::{nameof(CreateWorkflow)}";
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

        var uri = $"/methods/workflow.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
    
    [HttpGet("workflow.events", Name = nameof(ListWorkflowEvents))]
    public async Task<IActionResult> ListWorkflowEvents([FromQuery] ListWorkflowEventCommand command)
    {
        var msg = $"{Request.Method}::{nameof(WorkflowController)}::{nameof(ListWorkflowEvents)}";
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

        return Ok(new ListResult<WorkflowEvent>
        {
            Ok = true,
            Data = result.Value
        });
    }
}