namespace GreenFil.Application.DTOs;

public class Modelo3DCreateDTO
{
    public int UsuarioId { get; set; }
    public string NombreModelo { get; set; } = null!;
    public string RutaArchivo { get; set; } = null!;
    public string? ImagenPreview { get; set; }
}