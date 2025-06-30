using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using GreenFil.Infrastructure.GreenFilContext;
using Microsoft.EntityFrameworkCore;

namespace GreenFil.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly GreenFilContext.GreenfilContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(GreenFilContext.GreenfilContext context, IJwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    public async Task<LoginResultDTO?> LoginAsync(LoginDTO dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Passwordhash == dto.Password);

        if (usuario == null)
            return null;

        var token = _jwtService.GenerateToken(
            usuario.Id,
            usuario.Nombreusuario,
            usuario.Email,
            usuario.Rol
        );

        return new LoginResultDTO { Token = token };
    }
}