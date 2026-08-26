using System.ComponentModel.DataAnnotations;

namespace PremiumService.Application.Contracts;

public sealed record PremiumPlanDto(Guid PlanId, Guid PolicyTypeId, string Frequency, decimal BasePremium);
public sealed record CreatePremiumPlanRequest(
	Guid PolicyTypeId,
	[Required, MaxLength(30)] string Frequency,
	[Range(typeof(decimal), "0.01", "1000000000")] decimal BasePremium);

public sealed record PremiumScheduleDto(Guid ScheduleId, Guid PolicyId, DateOnly DueDate, decimal Amount, string Status);
public sealed record CreatePremiumScheduleRequest(
	Guid PolicyId,
	DateOnly DueDate,
	[Range(typeof(decimal), "0.01", "1000000000")] decimal Amount,
	[Required, MaxLength(30)] string Status);

public sealed record PremiumHistoryDto(Guid HistoryId, Guid PolicyId, DateTime PaidDate, decimal Amount);
public sealed record CreatePremiumHistoryRequest(
	Guid PolicyId,
	DateTime PaidDate,
	[Range(typeof(decimal), "0.01", "1000000000")] decimal Amount);

public sealed record PremiumDiscountDto(Guid DiscountId, Guid PolicyId, string DiscountType, decimal Percentage);
public sealed record CreatePremiumDiscountRequest(
	Guid PolicyId,
	[Required, MaxLength(50)] string DiscountType,
	[Range(typeof(decimal), "0", "100")] decimal Percentage);
