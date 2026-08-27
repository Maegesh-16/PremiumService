using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Premium_ServiceAPI.Authentication;

namespace Premium_ServiceAPI.Controllers;

[ApiController]
[Route("api/dev-auth")]
public class DevelopmentAuthController(IOptions<JwtSettings> jwtOptions, IWebHostEnvironment environment) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("token")]
    public ActionResult<object> CreateDevelopmentToken([FromQuery] string role = PremiumServiceRoles.Administrator)
    {
        if (!environment.IsDevelopment()) return NotFound();

        var settings = jwtOptions.Value;
        var token = new JwtSecurityToken(settings.Issuer, settings.Audience,
            [new Claim(JwtRegisteredClaimNames.Sub, "local-swagger-tester"), new Claim(ClaimTypes.Role, role)],
            expires: DateTime.UtcNow.AddHours(1), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey)), SecurityAlgorithms.HmacSha256));

        return Ok(new { accessToken = new JwtSecurityTokenHandler().WriteToken(token), expiresAt = token.ValidTo, role });
    }
}