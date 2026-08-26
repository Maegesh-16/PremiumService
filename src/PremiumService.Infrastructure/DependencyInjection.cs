using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PremiumService.Application.Contracts;
using PremiumService.Infrastructure.Persistence;
using PremiumService.Infrastructure.Services;

namespace PremiumService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPremiumInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PremiumDb")
            ?? throw new InvalidOperationException("Connection string 'PremiumDb' was not found.");

        services.AddDbContext<PremiumDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IPremiumManagementService, PremiumManagementService>();

        return services;
    }
}
