using Microsoft.EntityFrameworkCore;
using Premium_ServiceAPI.Models.Premium;

namespace Premium_ServiceAPI.Data;

public class PremiumDbContext(DbContextOptions<PremiumDbContext> options) : DbContext(options)
{
    public DbSet<PremiumPlan> PremiumPlans => Set<PremiumPlan>();
    public DbSet<PremiumSchedule> PremiumSchedules => Set<PremiumSchedule>();
    public DbSet<PremiumHistory> PremiumHistories => Set<PremiumHistory>();
    public DbSet<PremiumDiscount> PremiumDiscounts => Set<PremiumDiscount>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateOnly>().HaveColumnType("date");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PremiumDbContext).Assembly);
    }
}