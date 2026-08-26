namespace PremiumService.Domain.Entities;

public class PremiumHistory
{
    public Guid HistoryId { get; set; }
    public Guid PolicyId { get; set; }
    public DateTime PaidDate { get; set; }
    public decimal Amount { get; set; }
}
