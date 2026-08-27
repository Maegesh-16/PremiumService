using System.ComponentModel.DataAnnotations;

namespace Premium_ServiceAPI.DTOs.Premium;

public class PremiumPlanDto
{
    public Guid PlanId { get; set; }
    public Guid PolicyTypeId { get; set; }
    public string Frequency { get; set; } = string.Empty;
    public decimal BasePremium { get; set; }
}

public class CreatePremiumPlanDto
{
    [Required] public Guid PolicyTypeId { get; set; }
    [Required, StringLength(30)] public string Frequency { get; set; } = string.Empty;
    [Range(typeof(decimal), "0.01", "1000000000")] public decimal BasePremium { get; set; }
}

public class PremiumScheduleDto
{
    public Guid ScheduleId { get; set; }
    public Guid PolicyId { get; set; }
    public int InstallmentNumber { get; set; }
    public DateOnly DueDate { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid? PaymentId { get; set; }
    public DateTime? PaidDate { get; set; }
}

public class CreatePremiumScheduleDto
{
    [Required] public Guid PolicyId { get; set; }
    [Required] public DateOnly DueDate { get; set; }
    [Range(typeof(decimal), "0.01", "1000000000")] public decimal Amount { get; set; }
    [Required, StringLength(30)] public string Status { get; set; } = string.Empty;
}

public class PremiumHistoryDto
{
    public Guid HistoryId { get; set; }
    public Guid PolicyId { get; set; }
    public Guid PaymentId { get; set; }
    public DateTime PaidDate { get; set; }
    public decimal Amount { get; set; }
}

public class CreatePremiumHistoryDto
{
    [Required] public Guid PolicyId { get; set; }
    [Required] public DateTime PaidDate { get; set; }
    [Range(typeof(decimal), "0.01", "1000000000")] public decimal Amount { get; set; }
}

public class PremiumDiscountDto
{
    public Guid DiscountId { get; set; }
    public Guid PolicyId { get; set; }
    public string DiscountType { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
}

public class CreatePremiumDiscountDto
{
    [Required] public Guid PolicyId { get; set; }
    [Required, StringLength(50)] public string DiscountType { get; set; } = string.Empty;
    [Range(typeof(decimal), "0", "100")] public decimal Percentage { get; set; }
}

public class CalculatePremiumDto
{
    [Required] public Guid PolicyId { get; set; }
    [Required] public Guid PolicyTypeId { get; set; }
    [Required, StringLength(30)] public string Frequency { get; set; } = string.Empty;
}

public class PremiumCalculationDto
{
    public Guid PolicyId { get; set; }
    public Guid PlanId { get; set; }
    public decimal BasePremium { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal PayableAmount { get; set; }
}

public class GeneratePremiumSchedulesDto : CalculatePremiumDto
{
    [Required] public DateOnly PolicyStartDate { get; set; }
    [Required] public DateOnly PolicyEndDate { get; set; }
}

public class ConfirmPremiumPaymentDto
{
    [Required] public Guid PaymentId { get; set; }
    [Required] public DateTime PaidDate { get; set; }
    [Required, StringLength(30)] public string PaymentStatus { get; set; } = string.Empty;
}