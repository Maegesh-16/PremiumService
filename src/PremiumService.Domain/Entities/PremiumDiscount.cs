namespace PremiumService.Domain.Entities;

public class PremiumDiscount
{
    public Guid DiscountId { get; set; }
    public Guid PolicyId { get; set; }
    public string DiscountType { get; set; } = string.Empty;
    public decimal Percentage { get; set; }
}
