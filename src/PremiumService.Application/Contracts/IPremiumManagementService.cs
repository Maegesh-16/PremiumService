namespace PremiumService.Application.Contracts;

public interface IPremiumManagementService
{
    Task<IReadOnlyCollection<PremiumPlanDto>> GetPlansAsync(CancellationToken cancellationToken);
    Task<PremiumPlanDto> CreatePlanAsync(CreatePremiumPlanRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<PremiumScheduleDto>> GetSchedulesAsync(Guid? policyId, CancellationToken cancellationToken);
    Task<PremiumScheduleDto> CreateScheduleAsync(CreatePremiumScheduleRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<PremiumHistoryDto>> GetHistoryAsync(Guid? policyId, CancellationToken cancellationToken);
    Task<PremiumHistoryDto> CreateHistoryAsync(CreatePremiumHistoryRequest request, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<PremiumDiscountDto>> GetDiscountsAsync(Guid? policyId, CancellationToken cancellationToken);
    Task<PremiumDiscountDto> CreateDiscountAsync(CreatePremiumDiscountRequest request, CancellationToken cancellationToken);
}
