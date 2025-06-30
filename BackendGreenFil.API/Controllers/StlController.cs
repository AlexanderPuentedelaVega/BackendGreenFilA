using System.Security.Claims;
using greenfil_backend.DTOs;
using GreenFil.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace greenfil_backend.Controllers;

[ApiController]
[Route("api/stl")]
public class StlController : ControllerBase
{
    private readonly IModeloStlService _service;

    public StlController(IModeloStlService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpPost("generate")]
    public async Task<IActionResult> GenerateStl([FromForm] StlRequest request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        try
        {
            var (glbUrl, stlFileName) = await _service.GenerarDesdeImagenAsync(userId, request.ImageFile, request.NombreModelo);
            return Ok(new { glbUrl, stlFileName });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("generate-from-text")]
    public async Task<IActionResult> GenerateFromText([FromBody] SrlTextRequestDTO request)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        try
        {
            var (glbUrl, stlFileName) = await _service.GenerarDesdeTextoAsync(userId, request.Prompt, request.NombreModelo);
            return Ok(new { glbUrl, stlFileName });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
