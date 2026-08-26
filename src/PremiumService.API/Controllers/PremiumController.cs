using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PremiumService.Application.Contracts;

namespace PremiumService.API.Controllers;

[ApiController]
[Route("api/premium")]
[Authorize]
public class PremiumController(IPremiumManagementService premiumService) : ControllerBase
{
    [HttpGet("plans")]
    public async Task<ActionResult<IReadOnlyCollection<PremiumPlanDto>>> GetPlans(CancellationToken cancellationToken)
    {
        return Ok(await premiumService.GetPlansAsync(cancellationToken));
    }

    [HttpPost("plans")]
    [Authorize(Roles = "Admin,Finance")]
    public async Task<ActionResult<PremiumPlanDto>> CreatePlan([FromBody] CreatePremiumPlanRequest request, CancellationToken cancellationToken)
    {
        var result = await premiumService.CreatePlanAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetPlans), new { id = result.PlanId }, result);
    }

    [HttpGet("schedules")]
    public async Task<ActionResult<IReadOnlyCollection<PremiumScheduleDto>>> GetSchedules([FromQuery] Guid? policyId, CancellationToken cancellationToken)
    {
        return Ok(await premiumService.GetSchedulesAsync(policyId, cancellationToken));
    }

    [HttpPost("schedules")]
    [Authorize(Roles = "Admin,Finance")]
    public async Task<ActionResult<PremiumScheduleDto>> CreateSchedule([FromBody] CreatePremiumScheduleRequest request, CancellationToken cancellationToken)
    {
        var result = await premiumService.CreateScheduleAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetSchedules), new { policyId = result.PolicyId }, result);
    }

    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyCollection<PremiumHistoryDto>>> GetHistory([FromQuery] Guid? policyId, CancellationToken cancellationToken)
    {
        return Ok(await premiumService.GetHistoryAsync(policyId, cancellationToken));
    }

    [HttpPost("history")]
    [Authorize(Roles = "Admin,Finance")]
    public async Task<ActionResult<PremiumHistoryDto>> CreateHistory([FromBody] CreatePremiumHistoryRequest request, CancellationToken cancellationToken)
    {
        var result = await premiumService.CreateHistoryAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetHistory), new { policyId = result.PolicyId }, result);
    }

    [HttpGet("discounts")]
    public async Task<ActionResult<IReadOnlyCollection<PremiumDiscountDto>>> GetDiscounts([FromQuery] Guid? policyId, CancellationToken cancellationToken)
    {
        return Ok(await premiumService.GetDiscountsAsync(policyId, cancellationToken));
    }

    [HttpPost("discounts")]
    [Authorize(Roles = "Admin,Finance")]
    public async Task<ActionResult<PremiumDiscountDto>> CreateDiscount([FromBody] CreatePremiumDiscountRequest request, CancellationToken cancellationToken)
    {
        var result = await premiumService.CreateDiscountAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetDiscounts), new { policyId = result.PolicyId }, result);
    }

    [HttpGet("health")]
    [AllowAnonymous]
    public IActionResult Health() => Ok(new { status = "Premium service is running" });
}
