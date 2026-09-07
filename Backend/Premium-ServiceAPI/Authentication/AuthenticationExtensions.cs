using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Premium_ServiceAPI.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddPremiumServiceAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        var settings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings were not found.");

        if (string.IsNullOrWhiteSpace(settings.SecretKey) || settings.SecretKey.Length < 32)
        {
            throw new InvalidOperationException("JWT secret key must be at least 32 characters long.");
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = settings.ValidateIssuer,
                    ValidIssuer = settings.Issuer,
                    ValidateAudience = settings.ValidateAudience,
                    ValidAudience = settings.Audience,
                    ValidateLifetime = settings.ValidateLifetime,
                    ValidateIssuerSigningKey = settings.ValidateIssuerSigningKey,
                    IssuerSigningKey = signingKey,
                    ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
                    ClockSkew = TimeSpan.FromMinutes(settings.ClockSkewMinutes)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PremiumServicePolicies.PremiumRead, policy => policy.RequireAuthenticatedUser().RequireRole(PremiumServiceRoles.Customer, PremiumServiceRoles.Agent, PremiumServiceRoles.ClaimsOfficer, PremiumServiceRoles.CustomerSupport, PremiumServiceRoles.PlatformAdmin));
            options.AddPolicy(PremiumServicePolicies.PremiumManage, policy => policy.RequireAuthenticatedUser().RequireRole( PremiumServiceRoles.PlatformAdmin));
        });

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { Name = "Authorization", Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header, Description = "Enter JWT Bearer token. Example: Bearer {token}" });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() } });
        });

        return services;
    }
}