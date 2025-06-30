using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GreenFil.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GreenFil.Infrastructure.Services;

public class JwtService : IJwtService
{
    private readonly string _claveSecreta;

    public JwtService(IConfiguration config)
    {
        _claveSecreta = config["Jwt:Key"]!;
    }

    public string GenerateToken(int userId, string nombreUsuario, string email, string rol)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, nombreUsuario),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, rol),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_claveSecreta));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "tuApi",
            audience: "tuApi",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}