using Premium_ServiceAPI.DTOs.Premium;

namespace Premium_ServiceAPI.Services.Interfaces;

public interface IPremiumService
{
    Task<IReadOnlyList<PremiumPlanDto>> GetPlansAsync(CancellationToken cancellationToken = default);
    Task<PremiumPlanDto> CreatePlanAsync(CreatePremiumPlanDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PremiumScheduleDto>> GetSchedulesAsync(Guid? policyId, CancellationToken cancellationToken = default);
    Task<PremiumScheduleDto> CreateScheduleAsync(CreatePremiumScheduleDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PremiumHistoryDto>> GetHistoryAsync(Guid? policyId, CancellationToken cancellationToken = default);
    Task<PremiumHistoryDto> CreateHistoryAsync(CreatePremiumHistoryDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PremiumDiscountDto>> GetDiscountsAsync(Guid? policyId, CancellationToken cancellationToken = default);
    Task<PremiumDiscountDto> CreateDiscountAsync(CreatePremiumDiscountDto dto, CancellationToken cancellationToken = default);
    Task<PremiumCalculationDto> CalculateAsync(CalculatePremiumDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PremiumScheduleDto>> GenerateSchedulesAsync(GeneratePremiumSchedulesDto dto, CancellationToken cancellationToken = default);
    Task<PremiumScheduleDto> ConfirmPaymentAsync(Guid scheduleId, ConfirmPremiumPaymentDto dto, CancellationToken cancellationToken = default);
}