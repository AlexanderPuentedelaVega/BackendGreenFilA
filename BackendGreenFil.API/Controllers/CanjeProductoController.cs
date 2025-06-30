using System.Security.Claims;
using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CanjeProductoController : ControllerBase
{
    private readonly ICanjeProductoService _service;

    public CanjeProductoController(ICanjeProductoService service)
    {
        _service = service;
    }

    [HttpPost("canjear")]
    [Authorize]
    public async Task<IActionResult> CanjearProducto([FromBody] CanjeProductoDTO dto)
    {
        if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int usuarioId))
            return Unauthorized();

        var mensaje = await _service.CanjearAsync(usuarioId, dto);
        if (mensaje != "Canje exitoso.") return BadRequest(new { error = mensaje });

        return Ok(new { mensaje });
    }

    [HttpGet("pendientes")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetPendientes()
    {
        var pendientes = await _service.ObtenerPedidosPendientesAsync();
        return Ok(pendientes);
    }
}