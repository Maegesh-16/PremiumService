using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Premium_ServiceAPI.Authentication;
using Premium_ServiceAPI.DTOs.Premium;
using Premium_ServiceAPI.Services.Interfaces;

namespace Premium_ServiceAPI.Controllers;

[ApiController]
[Route("api/premium")]
[Authorize(Policy = PremiumServicePolicies.PremiumRead)]
public class PremiumController(IPremiumService premiumService) : PremiumControllerBase
{
    [HttpPost("calculate")]
    public async Task<ActionResult<PremiumCalculationDto>> CalculateAsync([FromBody] CalculatePremiumDto dto, CancellationToken cancellationToken)
    {
        try { return Ok(await premiumService.CalculateAsync(dto, cancellationToken)); }
        catch (InvalidOperationException exception) { return HandleInvalidOperation(exception); }
    }

    [HttpPost("schedules/generate")]
    [Authorize(Policy = PremiumServicePolicies.PremiumManage)]
    public async Task<ActionResult<IReadOnlyList<PremiumScheduleDto>>> GenerateSchedulesAsync([FromBody] GeneratePremiumSchedulesDto dto, CancellationToken cancellationToken)
    {
        try { return Ok(await premiumService.GenerateSchedulesAsync(dto, cancellationToken)); }
        catch (InvalidOperationException exception) { return HandleInvalidOperation(exception); }
    }

    [HttpPost("schedules/{scheduleId:guid}/payment-confirmation")]
    [Authorize(Policy = PremiumServicePolicies.PremiumManage)]
    public async Task<ActionResult<PremiumScheduleDto>> ConfirmPaymentAsync(Guid scheduleId, [FromBody] ConfirmPremiumPaymentDto dto, CancellationToken cancellationToken)
    {
        try { return Ok(await premiumService.ConfirmPaymentAsync(scheduleId, dto, cancellationToken)); }
        catch (InvalidOperationException exception) { return HandleInvalidOperation(exception); }
    }

    [HttpGet("plans")]
    public Task<IReadOnlyList<PremiumPlanDto>> GetPlansAsync(CancellationToken cancellationToken) => premiumService.GetPlansAsync(cancellationToken);
    [HttpPost("plans")]
    [Authorize(Policy = PremiumServicePolicies.PremiumManage)]
    public async Task<ActionResult<PremiumPlanDto>> CreatePlanAsync(CreatePremiumPlanDto dto, CancellationToken cancellationToken) => Ok(await premiumService.CreatePlanAsync(dto, cancellationToken));
    [HttpGet("schedules")]
    public Task<IReadOnlyList<PremiumScheduleDto>> GetSchedulesAsync([FromQuery] Guid? policyId, CancellationToken cancellationToken) => premiumService.GetSchedulesAsync(policyId, cancellationToken);
    [HttpPost("schedules")]
    [Authorize(Policy = PremiumServicePolicies.PremiumManage)]
    public async Task<ActionResult<PremiumScheduleDto>> CreateScheduleAsync(CreatePremiumScheduleDto dto, CancellationToken cancellationToken) => Ok(await premiumService.CreateScheduleAsync(dto, cancellationToken));
    [HttpGet("history")]
    public Task<IReadOnlyList<PremiumHistoryDto>> GetHistoryAsync([FromQuery] Guid? policyId, CancellationToken cancellationToken) => premiumService.GetHistoryAsync(policyId, cancellationToken);
    [HttpPost("history")]
    [Authorize(Policy = PremiumServicePolicies.PremiumManage)]
    public async Task<ActionResult<PremiumHistoryDto>> CreateHistoryAsync(CreatePremiumHistoryDto dto, CancellationToken cancellationToken) => Ok(await premiumService.CreateHistoryAsync(dto, cancellationToken));
    [HttpGet("discounts")]
    public Task<IReadOnlyList<PremiumDiscountDto>> GetDiscountsAsync([FromQuery] Guid? policyId, CancellationToken cancellationToken) => premiumService.GetDiscountsAsync(policyId, cancellationToken);
    [HttpPost("discounts")]
    [Authorize(Policy = PremiumServicePolicies.PremiumManage)]
    public async Task<ActionResult<PremiumDiscountDto>> CreateDiscountAsync(CreatePremiumDiscountDto dto, CancellationToken cancellationToken) => Ok(await premiumService.CreateDiscountAsync(dto, cancellationToken));
}