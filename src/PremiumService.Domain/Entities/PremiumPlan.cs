namespace PremiumService.Domain.Entities;

public class PremiumPlan
{
    public Guid PlanId { get; set; }
    public Guid PolicyTypeId { get; set; }
    public string Frequency { get; set; } = string.Empty;
    public decimal BasePremium { get; set; }
}
