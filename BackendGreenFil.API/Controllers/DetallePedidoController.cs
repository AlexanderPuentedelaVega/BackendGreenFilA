using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using GreenFil.Infrastructure.GreenFilContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DetallePedidoController : ControllerBase
{
    private readonly IDetallePedidoService _detalleService;

    public DetallePedidoController(IDetallePedidoService detalleService)
    {
        _detalleService = detalleService;
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetDetallePedidos()
    {
        var detalles = await _detalleService.ObtenerTodosAsync();
        return Ok(detalles);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetallePedido(int id)
    {
        var detalle = await _detalleService.ObtenerPorIdAsync(id);
        if (detalle == null) return NotFound();
        return Ok(detalle);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> PostDetallePedido(Detallepedido detalle)
    {
        var creado = await _detalleService.CrearAsync(detalle);
        return CreatedAtAction(nameof(GetDetallePedido), new { id = creado.Id }, creado);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDetallePedido(int id, Detallepedido detalle)
    {
        var actualizado = await _detalleService.ActualizarAsync(id, detalle);
        if (!actualizado) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDetallePedido(int id)
    {
        var eliminado = await _detalleService.EliminarAsync(id);
        if (!eliminado) return NotFound();
        return NoContent();
    }
    
    [HttpGet("debug")]
    public async Task<IActionResult> DebugDetallePedidos([FromServices] GreenfilContext context)
    {
        var detalles = await context.Detallepedidos
            .Include(d => d.Producto)
            .Include(d => d.Pedido)
            .ToListAsync();

        Console.WriteLine($"Debug: Se encontraron {detalles.Count} detalles.");

        return Ok(detalles);
    }
}