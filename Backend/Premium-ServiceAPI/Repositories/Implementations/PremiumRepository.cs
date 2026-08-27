using Microsoft.EntityFrameworkCore;
using Premium_ServiceAPI.Data;
using Premium_ServiceAPI.Models.Premium;
using Premium_ServiceAPI.Repositories.Interfaces;

namespace Premium_ServiceAPI.Repositories.Implementations;

public class PremiumRepository(PremiumDbContext context) : IPremiumRepository
{
    public async Task<IReadOnlyList<PremiumPlan>> GetPlansAsync(CancellationToken cancellationToken = default) =>
        await context.PremiumPlans.AsNoTracking().OrderBy(plan => plan.Frequency).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PremiumSchedule>> GetSchedulesAsync(Guid? policyId, CancellationToken cancellationToken = default) =>
        await FilterByPolicy(context.PremiumSchedules, policyId).AsNoTracking().OrderBy(schedule => schedule.DueDate).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PremiumHistory>> GetHistoryAsync(Guid? policyId, CancellationToken cancellationToken = default) =>
        await FilterByPolicy(context.PremiumHistories, policyId).AsNoTracking().OrderByDescending(history => history.PaidDate).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PremiumDiscount>> GetDiscountsAsync(Guid? policyId, CancellationToken cancellationToken = default) =>
        await FilterByPolicy(context.PremiumDiscounts, policyId).AsNoTracking().ToListAsync(cancellationToken);

    public Task<PremiumPlan?> GetPlanAsync(Guid policyTypeId, string frequency, CancellationToken cancellationToken = default) =>
        context.PremiumPlans.AsNoTracking().SingleOrDefaultAsync(plan => plan.PolicyTypeId == policyTypeId && plan.Frequency == frequency, cancellationToken);

    public Task<PremiumSchedule?> GetScheduleForUpdateAsync(Guid scheduleId, CancellationToken cancellationToken = default) =>
        context.PremiumSchedules.SingleOrDefaultAsync(schedule => schedule.ScheduleId == scheduleId, cancellationToken);

    public Task<PremiumHistory?> GetHistoryByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default) =>
        context.PremiumHistories.AsNoTracking().SingleOrDefaultAsync(history => history.PaymentId == paymentId, cancellationToken);

    public Task<bool> ScheduleExistsAsync(Guid policyId, DateOnly dueDate, CancellationToken cancellationToken = default) =>
        context.PremiumSchedules.AnyAsync(schedule => schedule.PolicyId == policyId && schedule.DueDate == dueDate, cancellationToken);

    public Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class => context.Set<T>().AddAsync(entity, cancellationToken).AsTask();
    public Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class => context.Set<T>().AddRangeAsync(entities, cancellationToken);
    public void Update<T>(T entity) where T : class => context.Set<T>().Update(entity);
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => context.SaveChangesAsync(cancellationToken);

    private static IQueryable<T> FilterByPolicy<T>(IQueryable<T> query, Guid? policyId) where T : class =>
        policyId is null ? query : query.Where(entity => EF.Property<Guid>(entity, "PolicyId") == policyId);
}