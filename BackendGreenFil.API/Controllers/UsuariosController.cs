using System.Security.Claims;
using GreenFil.Domain.Entities;
using GreenFil.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace greenfil_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public UsuariosController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioService.ObtenerTodosAsync();
        return Ok(usuarios);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(id);
        if (usuario == null) return NotFound();
        return Ok(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Usuario usuario)
    {
        var dto = await _usuarioService.CrearUsuarioAsync(usuario);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, dto);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Usuario updated)
    {
        var ok = await _usuarioService.ActualizarUsuarioAsync(id, updated);
        if (!ok) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _usuarioService.EliminarUsuarioAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim == null) return Unauthorized();

        if (!int.TryParse(claim.Value, out int userId)) return BadRequest();

        var usuario = await _usuarioService.ObtenerPorIdAsync(userId);
        if (usuario == null) return NotFound();

        return Ok(new
        {
            usuario.Id,
            usuario.Nombreusuario,
            usuario.Email,
            usuario.Puntos,
            usuario.Rol
        });
    }
}