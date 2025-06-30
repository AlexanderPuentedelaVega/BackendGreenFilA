using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenFil.Application.DTOs;

public class ModelosPremiumUploadRequest
{
    [FromForm]
    public IFormFile ArchivoGLB { get; set; } = null!;

    [FromForm]
    public IFormFile ArchivoSTL { get; set; } = null!;

    [FromForm]
    public IFormFile ImagenPreview { get; set; } = null!;

    [FromForm]
    public string NombreModelo { get; set; } = null!;

    [FromForm]
    public string? Descripcion { get; set; }

    [FromForm]
    public decimal PrecioSoles { get; set; }

    [FromForm]
    public int PrecioPuntos { get; set; }

    [FromForm]
    public string Estado { get; set; } = "pendiente";
}