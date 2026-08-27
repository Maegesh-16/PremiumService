using Premium_ServiceAPI.DTOs.Premium;
using Premium_ServiceAPI.Models.Premium;

namespace Premium_ServiceAPI.Mappers;

public static class PremiumMappingExtensions
{
    public static PremiumPlanDto ToDto(this PremiumPlan plan) => new() { PlanId = plan.PlanId, PolicyTypeId = plan.PolicyTypeId, Frequency = plan.Frequency, BasePremium = plan.BasePremium };
    public static PremiumScheduleDto ToDto(this PremiumSchedule schedule) => new() { ScheduleId = schedule.ScheduleId, PolicyId = schedule.PolicyId, InstallmentNumber = schedule.InstallmentNumber, DueDate = schedule.DueDate, Amount = schedule.Amount, Status = schedule.Status, PaymentId = schedule.PaymentId, PaidDate = schedule.PaidDate };
    public static PremiumHistoryDto ToDto(this PremiumHistory history) => new() { HistoryId = history.HistoryId, PolicyId = history.PolicyId, PaymentId = history.PaymentId, PaidDate = history.PaidDate, Amount = history.Amount };
    public static PremiumDiscountDto ToDto(this PremiumDiscount discount) => new() { DiscountId = discount.DiscountId, PolicyId = discount.PolicyId, DiscountType = discount.DiscountType, Percentage = discount.Percentage };
}