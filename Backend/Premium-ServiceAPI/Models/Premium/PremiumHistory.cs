namespace Premium_ServiceAPI.Models.Premium;

public class PremiumHistory
{
    public Guid HistoryId { get; set; }
    public Guid PolicyId { get; set; }
    public Guid PaymentId { get; set; }
    public DateTime PaidDate { get; set; }
    public decimal Amount { get; set; }
}