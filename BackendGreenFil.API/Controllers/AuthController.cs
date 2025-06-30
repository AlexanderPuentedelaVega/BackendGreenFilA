using System.Security.Claims;
using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace greenfil_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(new { mensaje = "Credenciales inválidas" });

        return Ok(result);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("test")]
    public IActionResult TestAdmin()
    {
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        return Ok(new
        {
            mensaje = $"Hola {name}, tienes acceso como ADMIN ✔"
        });
    }
}