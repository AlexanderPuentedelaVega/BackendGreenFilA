using GreenFil.Application.DTOs;
using GreenFil.Application.Interfaces;
using GreenFil.Domain.Entities;
using GreenFil.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace greenfil_backend.Controllers;


[ApiController]
[Route("api/modelospremium")]
[Produces("application/json")]
public class ModelosPremiumController : ControllerBase
{
    private readonly IModeloPremiumService _service;
    private readonly IFileStorageService _fileStorageService;

    public ModelosPremiumController(IModeloPremiumService service, IFileStorageService fileStorageService)
    {
        _service = service;
        _fileStorageService = fileStorageService;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> UploadModeloPremium([FromForm] ModelosPremiumUploadRequest request)
    {
        var id = await _service.SubirYRegistrarModeloAsync(request);
        return Ok(new { mensaje = "Modelo premium registrado correctamente.", id });
    }

    [HttpGet("catalogo")]
    public async Task<IActionResult> GetCatalogo()
    {
        var modelos = await _service.ObtenerCatalogoAsync();
        return Ok(modelos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetModeloPorId(int id)
    {
        var modelo = await _service.ObtenerPorIdAsync(id);
        if (modelo == null) return NotFound("Modelo no encontrado.");
        return Ok(modelo);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModelo(int id, [FromBody] Modelospremium actualizado)
    {
        var result = await _service.ActualizarAsync(id, actualizado);
        if (!result) return NotFound("Modelo no encontrado.");
        return Ok(new { mensaje = "Modelo actualizado correctamente." });
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteModelo(int id)
    {
        var result = await _service.EliminarAsync(id);
        if (!result) return NotFound("Modelo no encontrado.");
        return Ok(new { mensaje = "Modelo eliminado correctamente." });
    }
    
}