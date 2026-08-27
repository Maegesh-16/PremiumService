using Premium_ServiceAPI.Enums;
using Premium_ServiceAPI.DTOs.Premium;
using Premium_ServiceAPI.Mappers;
using Premium_ServiceAPI.Models.Premium;
using Premium_ServiceAPI.Repositories.Interfaces;
using Premium_ServiceAPI.Services.Interfaces;

namespace Premium_ServiceAPI.Services.Implementations;

public class PremiumService(IPremiumRepository premiumRepository) : IPremiumService
{
    public async Task<IReadOnlyList<PremiumPlanDto>> GetPlansAsync(CancellationToken cancellationToken = default) => (await premiumRepository.GetPlansAsync(cancellationToken)).Select(plan => plan.ToDto()).ToList();
    public async Task<IReadOnlyList<PremiumScheduleDto>> GetSchedulesAsync(Guid? policyId, CancellationToken cancellationToken = default) => (await premiumRepository.GetSchedulesAsync(policyId, cancellationToken)).Select(schedule => schedule.ToDto()).ToList();
    public async Task<IReadOnlyList<PremiumHistoryDto>> GetHistoryAsync(Guid? policyId, CancellationToken cancellationToken = default) => (await premiumRepository.GetHistoryAsync(policyId, cancellationToken)).Select(history => history.ToDto()).ToList();
    public async Task<IReadOnlyList<PremiumDiscountDto>> GetDiscountsAsync(Guid? policyId, CancellationToken cancellationToken = default) => (await premiumRepository.GetDiscountsAsync(policyId, cancellationToken)).Select(discount => discount.ToDto()).ToList();

    public Task<PremiumPlanDto> CreatePlanAsync(CreatePremiumPlanDto dto, CancellationToken cancellationToken = default) => CreateAsync(new PremiumPlan { PlanId = Guid.NewGuid(), PolicyTypeId = dto.PolicyTypeId, Frequency = dto.Frequency, BasePremium = dto.BasePremium }, entity => entity.ToDto(), cancellationToken);
    public Task<PremiumScheduleDto> CreateScheduleAsync(CreatePremiumScheduleDto dto, CancellationToken cancellationToken = default) => CreateAsync(new PremiumSchedule { ScheduleId = Guid.NewGuid(), PolicyId = dto.PolicyId, DueDate = dto.DueDate, Amount = dto.Amount, Status = dto.Status }, entity => entity.ToDto(), cancellationToken);
    public Task<PremiumHistoryDto> CreateHistoryAsync(CreatePremiumHistoryDto dto, CancellationToken cancellationToken = default) => CreateAsync(new PremiumHistory { HistoryId = Guid.NewGuid(), PolicyId = dto.PolicyId, PaidDate = dto.PaidDate, Amount = dto.Amount }, entity => entity.ToDto(), cancellationToken);
    public Task<PremiumDiscountDto> CreateDiscountAsync(CreatePremiumDiscountDto dto, CancellationToken cancellationToken = default) => CreateAsync(new PremiumDiscount { DiscountId = Guid.NewGuid(), PolicyId = dto.PolicyId, DiscountType = dto.DiscountType, Percentage = dto.Percentage }, entity => entity.ToDto(), cancellationToken);

    public async Task<PremiumCalculationDto> CalculateAsync(CalculatePremiumDto dto, CancellationToken cancellationToken = default)
    {
        var frequency = NormalizeFrequency(dto.Frequency);
        var plan = await premiumRepository.GetPlanAsync(dto.PolicyTypeId, frequency, cancellationToken)
            ?? throw new InvalidOperationException("No premium plan exists for the policy type and frequency.");
        var discounts = await premiumRepository.GetDiscountsAsync(dto.PolicyId, cancellationToken);
        var discountPercentage = discounts.Sum(discount => discount.Percentage);

        if (discountPercentage > 100)
        {
            throw new InvalidOperationException("The total discount percentage cannot exceed 100.");
        }

        var discountAmount = decimal.Round(plan.BasePremium * discountPercentage / 100, 2, MidpointRounding.AwayFromZero);
        return new PremiumCalculationDto { PolicyId = dto.PolicyId, PlanId = plan.PlanId, BasePremium = plan.BasePremium, DiscountPercentage = discountPercentage, DiscountAmount = discountAmount, PayableAmount = plan.BasePremium - discountAmount };
    }

    public async Task<IReadOnlyList<PremiumScheduleDto>> GenerateSchedulesAsync(GeneratePremiumSchedulesDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.PolicyEndDate < dto.PolicyStartDate)
        {
            throw new InvalidOperationException("The policy end date must not be before the policy start date.");
        }

        var calculation = await CalculateAsync(dto, cancellationToken);
        var intervalMonths = GetFrequencyIntervalMonths(dto.Frequency);
        var schedules = new List<PremiumSchedule>();
        var dueDate = dto.PolicyStartDate;
        var installmentNumber = 1;

        while (dueDate <= dto.PolicyEndDate)
        {
            if (await premiumRepository.ScheduleExistsAsync(dto.PolicyId, dueDate, cancellationToken))
            {
                throw new InvalidOperationException($"A premium schedule already exists for {dueDate:yyyy-MM-dd}.");
            }

            schedules.Add(new PremiumSchedule { ScheduleId = Guid.NewGuid(), PolicyId = dto.PolicyId, InstallmentNumber = installmentNumber++, DueDate = dueDate, Amount = calculation.PayableAmount, Status = dueDate < DateOnly.FromDateTime(DateTime.UtcNow) ? PremiumScheduleStatus.Overdue : PremiumScheduleStatus.Pending });
            dueDate = dueDate.AddMonths(intervalMonths);
        }

        await premiumRepository.AddRangeAsync(schedules, cancellationToken);
        await premiumRepository.SaveChangesAsync(cancellationToken);
        return schedules.Select(schedule => schedule.ToDto()).ToList();
    }

    public async Task<PremiumScheduleDto> ConfirmPaymentAsync(Guid scheduleId, ConfirmPremiumPaymentDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.PaymentStatus is not ("Paid" or "Succeeded" or "Completed"))
        {
            throw new InvalidOperationException("Only a successful payment can be applied to a premium schedule.");
        }

        var existingHistory = await premiumRepository.GetHistoryByPaymentIdAsync(dto.PaymentId, cancellationToken);
        if (existingHistory is not null)
        {
            var existingSchedule = await premiumRepository.GetScheduleForUpdateAsync(scheduleId, cancellationToken);
            if (existingSchedule?.PaymentId == dto.PaymentId) return existingSchedule.ToDto();
            throw new InvalidOperationException("The payment has already been applied to another premium schedule.");
        }

        var schedule = await premiumRepository.GetScheduleForUpdateAsync(scheduleId, cancellationToken)
            ?? throw new InvalidOperationException("Premium schedule was not found.");
        if (!PremiumScheduleStatus.IsPayable(schedule.Status)) throw new InvalidOperationException("Only pending, due, or overdue schedules can be paid.");

        schedule.Status = PremiumScheduleStatus.Paid;
        schedule.PaymentId = dto.PaymentId;
        schedule.PaidDate = dto.PaidDate;
        await premiumRepository.AddAsync(new PremiumHistory { HistoryId = Guid.NewGuid(), PolicyId = schedule.PolicyId, PaymentId = dto.PaymentId, PaidDate = dto.PaidDate, Amount = schedule.Amount }, cancellationToken);
        premiumRepository.Update(schedule);
        await premiumRepository.SaveChangesAsync(cancellationToken);
        return schedule.ToDto();
    }

    private async Task<TDto> CreateAsync<TEntity, TDto>(TEntity entity, Func<TEntity, TDto> map, CancellationToken cancellationToken) where TEntity : class
    {
        await premiumRepository.AddAsync(entity, cancellationToken);
        await premiumRepository.SaveChangesAsync(cancellationToken);
        return map(entity);
    }

    private static string NormalizeFrequency(string frequency) => frequency.Trim();

    private static int GetFrequencyIntervalMonths(string frequency) => NormalizeFrequency(frequency).ToUpperInvariant() switch
    {
        "MONTHLY" => 1,
        "QUARTERLY" => 3,
        "HALF-YEARLY" or "HALFYEARLY" => 6,
        "YEARLY" or "ANNUAL" => 12,
        _ => throw new InvalidOperationException("Frequency must be Monthly, Quarterly, Half-Yearly, or Yearly.")
    };
}