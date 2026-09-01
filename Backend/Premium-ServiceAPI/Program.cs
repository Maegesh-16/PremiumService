using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Premium_ServiceAPI.Authentication;
using Premium_ServiceAPI.Data;
using Premium_ServiceAPI.Middleware;
using Premium_ServiceAPI.Repositories.Implementations;
using Premium_ServiceAPI.Repositories.Interfaces;
using Premium_ServiceAPI.Services.Implementations;
using Premium_ServiceAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://*:{port}");
}

var connectionString = builder.Configuration.GetConnectionString("PremiumServiceDb")
    ?? throw new InvalidOperationException("Connection string 'PremiumServiceDb' was not found.");

builder.Services.AddDbContext<PremiumDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IPremiumRepository, PremiumRepository>();
builder.Services.AddScoped<IPremiumService, PremiumService>();
builder.Services.AddPremiumServiceAuthentication(builder.Configuration);
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
var publicBaseUrl = builder.Configuration["AppUrls:PublicBaseUrl"]?.TrimEnd('/');

app.UseForwardedHeaders();
app.UseSwagger(options =>
{
    options.PreSerializeFilters.Add((swagger, httpRequest) =>
    {
        var serverUrl = !string.IsNullOrWhiteSpace(publicBaseUrl)
            ? publicBaseUrl
            : $"{httpRequest.Scheme}://{httpRequest.Host.Value}";

        swagger.Servers =
        [
            new OpenApiServer
            {
                Url = serverUrl
            }
        ];
    });
});
app.UseSwaggerUI();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PremiumDbContext>();
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception exception)
    {
        app.Logger.LogError(exception, "Premium database migration failed during startup.");
    }
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "Premium Service API" })).AllowAnonymous();
app.MapGet("/", () => Results.Redirect("/swagger")).AllowAnonymous();
app.MapControllers();
app.Run();