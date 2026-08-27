using Microsoft.AspNetCore.Mvc;

namespace Premium_ServiceAPI.Controllers;

[ApiController]
public abstract class PremiumControllerBase : ControllerBase
{
    protected ActionResult HandleInvalidOperation(InvalidOperationException exception)
    {
        return BadRequest(new ProblemDetails
        {
            Title = "Premium service validation failed.",
            Detail = exception.Message,
            Status = StatusCodes.Status400BadRequest
        });
    }
}