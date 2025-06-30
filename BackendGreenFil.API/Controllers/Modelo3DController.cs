
using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Modelo3DController : ControllerBase
{
    private readonly IModelo3DService _service;

    public Modelo3DController(IModelo3DService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetModelos()
    {
        var modelos = await _service.ObtenerTodosAsync();
        return Ok(modelos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetModelo(int id)
    {
        var modelo = await _service.ObtenerPorIdAsync(id);
        if (modelo == null) return NotFound();
        return Ok(modelo);
    }

    [HttpPost]
    public async Task<IActionResult> PostModelo([FromBody] Modelo3DCreateDTO dto)
    {
        var id = await _service.CrearAsync(dto);
        return CreatedAtAction(nameof(GetModelo), new { id }, new { mensaje = "Modelo creado", id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutModelo(int id, [FromBody] Modelo3DCreateDTO dto)
    {
        var actualizado = await _service.ActualizarAsync(id, dto);
        if (!actualizado) return NotFound();
        return Ok(new { mensaje = "Modelo actualizado" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModelo(int id)
    {
        var eliminado = await _service.EliminarAsync(id);
        if (!eliminado) return NotFound();
        return Ok(new { mensaje = "Modelo eliminado" });
    }
}