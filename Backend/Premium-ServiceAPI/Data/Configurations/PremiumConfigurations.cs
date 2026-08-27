using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Premium_ServiceAPI.Models.Premium;

namespace Premium_ServiceAPI.Data.Configurations;

public class PremiumPlanConfiguration : IEntityTypeConfiguration<PremiumPlan>
{
    public void Configure(EntityTypeBuilder<PremiumPlan> builder)
    {
        builder.ToTable("PremiumPlans");
        builder.HasKey(plan => plan.PlanId);
        builder.Property(plan => plan.Frequency).HasMaxLength(30).IsRequired();
        builder.Property(plan => plan.BasePremium).HasPrecision(18, 2);
        builder.HasIndex(plan => plan.PolicyTypeId);
    }
}

public class PremiumScheduleConfiguration : IEntityTypeConfiguration<PremiumSchedule>
{
    public void Configure(EntityTypeBuilder<PremiumSchedule> builder)
    {
        builder.ToTable("PremiumSchedules");
        builder.HasKey(schedule => schedule.ScheduleId);
        builder.Property(schedule => schedule.InstallmentNumber).IsRequired();
        builder.Property(schedule => schedule.Status).HasMaxLength(30).IsRequired();
        builder.Property(schedule => schedule.Amount).HasPrecision(18, 2);
        builder.HasIndex(schedule => schedule.PolicyId);
        builder.HasIndex(schedule => new { schedule.PolicyId, schedule.DueDate }).IsUnique();
        builder.HasIndex(schedule => schedule.PaymentId).IsUnique().HasFilter("[PaymentId] IS NOT NULL");
    }
}

public class PremiumHistoryConfiguration : IEntityTypeConfiguration<PremiumHistory>
{
    public void Configure(EntityTypeBuilder<PremiumHistory> builder)
    {
        builder.ToTable("PremiumHistories");
        builder.HasKey(history => history.HistoryId);
        builder.Property(history => history.Amount).HasPrecision(18, 2);
        builder.HasIndex(history => history.PolicyId);
        builder.HasIndex(history => history.PaymentId).IsUnique();
    }
}

public class PremiumDiscountConfiguration : IEntityTypeConfiguration<PremiumDiscount>
{
    public void Configure(EntityTypeBuilder<PremiumDiscount> builder)
    {
        builder.ToTable("PremiumDiscounts");
        builder.HasKey(discount => discount.DiscountId);
        builder.Property(discount => discount.DiscountType).HasMaxLength(50).IsRequired();
        builder.Property(discount => discount.Percentage).HasPrecision(5, 2);
        builder.HasIndex(discount => discount.PolicyId);
    }
}