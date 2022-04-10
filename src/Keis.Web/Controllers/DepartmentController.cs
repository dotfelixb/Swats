using MediatR;
using Microsoft.AspNetCore.Mvc;
using Keis.Model;
using Keis.Model.Commands;
using Keis.Model.Queries;
using Keis.Web.Extensions;

namespace Keis.Web.Controllers;

public class DepartmentController : MethodController
{
    private readonly ILogger<DepartmentController> logger;
    private readonly IMediator mediatr;

    public DepartmentController(ILogger<DepartmentController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpGet("department.list", Name = nameof(ListDepartments))]
    public async Task<IActionResult> ListDepartments([FromQuery] ListDepartmentCommand command)
    {
        var msg = $"{Request.Method}::{nameof(DepartmentController)}::{nameof(ListDepartments)}";
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

        return Ok(new ListResult<FetchDepartment>
        {
            Ok = true,
            Data = result.Value
        });
    }
    
    [HttpGet("department.get", Name = nameof(GetDepartment))]
    public async Task<IActionResult> GetDepartment([FromQuery] GetDepartmentCommand command)
    {
        var msg = $"{Request.Method}::{nameof(DepartmentController)}::{nameof(GetDepartment)}";
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

        return Ok(new SingleResult<FetchDepartment>
        {
            Ok = true,
            Data = result.Value
        });
    }
    
    [HttpPost("department.create", Name = nameof(CreateDepartment))]
    public async Task<IActionResult> CreateDepartment( CreateDepartmentCommand command)
    {
        var msg = $"{Request.Method}::{nameof(DepartmentController)}::{nameof(CreateDepartment)}";
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

        var uri = $"/methods/department.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}