using Premium_ServiceAPI.Models.Premium;

namespace Premium_ServiceAPI.Repositories.Interfaces;

public interface IPremiumRepository
{
    Task<IReadOnlyList<PremiumPlan>> GetPlansAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PremiumSchedule>> GetSchedulesAsync(Guid? policyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PremiumHistory>> GetHistoryAsync(Guid? policyId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PremiumDiscount>> GetDiscountsAsync(Guid? policyId, CancellationToken cancellationToken = default);
    Task<PremiumPlan?> GetPlanAsync(Guid policyTypeId, string frequency, CancellationToken cancellationToken = default);
    Task<PremiumSchedule?> GetScheduleForUpdateAsync(Guid scheduleId, CancellationToken cancellationToken = default);
    Task<PremiumHistory?> GetHistoryByPaymentIdAsync(Guid paymentId, CancellationToken cancellationToken = default);
    Task<bool> ScheduleExistsAsync(Guid policyId, DateOnly dueDate, CancellationToken cancellationToken = default);
    Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;
    Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : class;
    void Update<T>(T entity) where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}