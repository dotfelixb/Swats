using Keis.Infrastructure.Features.Department.CreateDepartment;
using Keis.Infrastructure.Features.Department.GetDepartment;
using Keis.Infrastructure.Features.Department.ListDepartment;
using Keis.Infrastructure.Features.Department.UpdateDepartment;
using Keis.Model;
using Keis.Model.Queries;
using Keis.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        const string msg = $"GET::{nameof(DepartmentController)}::{nameof(ListDepartments)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new ListResult<FetchDepartment>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpGet("department.get", Name = nameof(GetDepartment))]
    public async Task<IActionResult> GetDepartment([FromQuery] GetDepartmentCommand command)
    {
        const string msg = $"GET::{nameof(DepartmentController)}::{nameof(GetDepartment)}";
        logger.LogInformation(msg);

        var result = await mediatr.Send(command);
        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        return Ok(new SingleResult<FetchDepartment>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPost("department.create", Name = nameof(CreateDepartment))]
    public async Task<IActionResult> CreateDepartment(CreateDepartmentCommand command)
    {
        const string msg = $"POST::{nameof(DepartmentController)}::{nameof(CreateDepartment)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/department.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }

    [HttpPatch("department.update", Name = nameof(UpdateDepartment))]
    public async Task<IActionResult> UpdateDepartment(UpdateDepartmentCommand command)
    {
        const string msg = $"PATCH::{nameof(DepartmentController)}::{nameof(UpdateDepartment)}";
        logger.LogInformation(msg);

        command.UpdatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = result.Reasons.Select(s => s.Message)
            });

        var uri = $"/methods/department.get?id={result.Value}";
        return Created(uri, new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}