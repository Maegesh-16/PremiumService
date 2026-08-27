namespace Premium_ServiceAPI.Models.Premium;

public class PremiumSchedule
{
    public Guid ScheduleId { get; set; }
    public Guid PolicyId { get; set; }
    public int InstallmentNumber { get; set; }
    public DateOnly DueDate { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
    public Guid? PaymentId { get; set; }
    public DateTime? PaidDate { get; set; }
}