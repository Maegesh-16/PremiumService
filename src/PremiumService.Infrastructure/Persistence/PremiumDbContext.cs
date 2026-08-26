using Microsoft.EntityFrameworkCore;
using PremiumService.Domain.Entities;

namespace PremiumService.Infrastructure.Persistence;

public class PremiumDbContext(DbContextOptions<PremiumDbContext> options) : DbContext(options)
{
    public DbSet<PremiumPlan> PremiumPlans => Set<PremiumPlan>();
    public DbSet<PremiumSchedule> PremiumSchedules => Set<PremiumSchedule>();
    public DbSet<PremiumHistory> PremiumHistories => Set<PremiumHistory>();
    public DbSet<PremiumDiscount> PremiumDiscounts => Set<PremiumDiscount>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PremiumPlan>().HasKey(x => x.PlanId);
        modelBuilder.Entity<PremiumSchedule>().HasKey(x => x.ScheduleId);
        modelBuilder.Entity<PremiumHistory>().HasKey(x => x.HistoryId);
        modelBuilder.Entity<PremiumDiscount>().HasKey(x => x.DiscountId);

        modelBuilder.Entity<PremiumPlan>().Property(x => x.BasePremium).HasPrecision(18, 2);
        modelBuilder.Entity<PremiumSchedule>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<PremiumHistory>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<PremiumDiscount>().Property(x => x.Percentage).HasPrecision(5, 2);
    }
}
