namespace PremiumService.Domain.Entities;

public class PremiumSchedule
{
    public Guid ScheduleId { get; set; }
    public Guid PolicyId { get; set; }
    public DateOnly DueDate { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Pending";
}
