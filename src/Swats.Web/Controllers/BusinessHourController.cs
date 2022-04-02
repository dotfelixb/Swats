using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swats.Model;
using Swats.Model.Commands;
using Swats.Web.Extensions;

namespace Swats.Web.Controllers;

public class BusinessHourController : MethodController
{
    private readonly ILogger<BusinessHourController> logger;
    private readonly IMediator mediatr;

    public BusinessHourController(ILogger<BusinessHourController> logger, IMediator mediatr)
    {
        this.logger = logger;
        this.mediatr = mediatr;
    }

    [HttpPost("businesshour.create", Name = nameof(CreateHour))]
    public async Task<IActionResult> CreateHour(CreateBusinessHourCommand command)
    {
        var msg = $"{Request.Method}::{nameof(BusinessHourController)}::{nameof(CreateHour)}";
        logger.LogInformation(msg);

        command.CreatedBy = Request.HttpContext.UserId();
        var result = await mediatr.Send(command);

        if (result.IsFailed)
        {
            var err = result.Reasons.FirstOrDefault()?.Message;
            return BadRequest(new ErrorResult
            {
                Ok = false,
                Errors = new[] { err },
            });
        }

        return Created("", new SingleResult<string>
        {
            Ok = true,
            Data = result.Value
        });
    }
}