using Microsoft.EntityFrameworkCore;
using PremiumService.Application.Contracts;
using PremiumService.Domain.Entities;
using PremiumService.Infrastructure.Persistence;

namespace PremiumService.Infrastructure.Services;

public class PremiumManagementService(PremiumDbContext dbContext) : IPremiumManagementService
{
    public async Task<IReadOnlyCollection<PremiumPlanDto>> GetPlansAsync(CancellationToken cancellationToken)
    {
        return await dbContext.PremiumPlans
            .AsNoTracking()
            .Select(x => new PremiumPlanDto(x.PlanId, x.PolicyTypeId, x.Frequency, x.BasePremium))
            .ToListAsync(cancellationToken);
    }

    public async Task<PremiumPlanDto> CreatePlanAsync(CreatePremiumPlanRequest request, CancellationToken cancellationToken)
    {
        var entity = new PremiumPlan
        {
            PlanId = Guid.NewGuid(),
            PolicyTypeId = request.PolicyTypeId,
            Frequency = request.Frequency,
            BasePremium = request.BasePremium
        };

        dbContext.PremiumPlans.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new PremiumPlanDto(entity.PlanId, entity.PolicyTypeId, entity.Frequency, entity.BasePremium);
    }

    public async Task<IReadOnlyCollection<PremiumScheduleDto>> GetSchedulesAsync(Guid? policyId, CancellationToken cancellationToken)
    {
        var query = dbContext.PremiumSchedules.AsNoTracking();

        if (policyId.HasValue)
        {
            query = query.Where(x => x.PolicyId == policyId.Value);
        }

        return await query
            .Select(x => new PremiumScheduleDto(x.ScheduleId, x.PolicyId, x.DueDate, x.Amount, x.Status))
            .ToListAsync(cancellationToken);
    }

    public async Task<PremiumScheduleDto> CreateScheduleAsync(CreatePremiumScheduleRequest request, CancellationToken cancellationToken)
    {
        var entity = new PremiumSchedule
        {
            ScheduleId = Guid.NewGuid(),
            PolicyId = request.PolicyId,
            DueDate = request.DueDate,
            Amount = request.Amount,
            Status = request.Status
        };

        dbContext.PremiumSchedules.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new PremiumScheduleDto(entity.ScheduleId, entity.PolicyId, entity.DueDate, entity.Amount, entity.Status);
    }

    public async Task<IReadOnlyCollection<PremiumHistoryDto>> GetHistoryAsync(Guid? policyId, CancellationToken cancellationToken)
    {
        var query = dbContext.PremiumHistories.AsNoTracking();

        if (policyId.HasValue)
        {
            query = query.Where(x => x.PolicyId == policyId.Value);
        }

        return await query
            .OrderByDescending(x => x.PaidDate)
            .Select(x => new PremiumHistoryDto(x.HistoryId, x.PolicyId, x.PaidDate, x.Amount))
            .ToListAsync(cancellationToken);
    }

    public async Task<PremiumHistoryDto> CreateHistoryAsync(CreatePremiumHistoryRequest request, CancellationToken cancellationToken)
    {
        var entity = new PremiumHistory
        {
            HistoryId = Guid.NewGuid(),
            PolicyId = request.PolicyId,
            PaidDate = request.PaidDate,
            Amount = request.Amount
        };

        dbContext.PremiumHistories.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new PremiumHistoryDto(entity.HistoryId, entity.PolicyId, entity.PaidDate, entity.Amount);
    }

    public async Task<IReadOnlyCollection<PremiumDiscountDto>> GetDiscountsAsync(Guid? policyId, CancellationToken cancellationToken)
    {
        var query = dbContext.PremiumDiscounts.AsNoTracking();

        if (policyId.HasValue)
        {
            query = query.Where(x => x.PolicyId == policyId.Value);
        }

        return await query
            .Select(x => new PremiumDiscountDto(x.DiscountId, x.PolicyId, x.DiscountType, x.Percentage))
            .ToListAsync(cancellationToken);
    }

    public async Task<PremiumDiscountDto> CreateDiscountAsync(CreatePremiumDiscountRequest request, CancellationToken cancellationToken)
    {
        var entity = new PremiumDiscount
        {
            DiscountId = Guid.NewGuid(),
            PolicyId = request.PolicyId,
            DiscountType = request.DiscountType,
            Percentage = request.Percentage
        };

        dbContext.PremiumDiscounts.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new PremiumDiscountDto(entity.DiscountId, entity.PolicyId, entity.DiscountType, entity.Percentage);
    }
}
