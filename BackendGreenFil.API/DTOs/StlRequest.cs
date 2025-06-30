using Microsoft.AspNetCore.Mvc;

namespace greenfil_backend.DTOs;

public class StlRequest
{
    [FromForm]
    public IFormFile ImageFile { get; set; } = null!;

    [FromForm]
    public int UsuarioId { get; set; }

    [FromForm]
    public string NombreModelo { get; set; } = null!;
}