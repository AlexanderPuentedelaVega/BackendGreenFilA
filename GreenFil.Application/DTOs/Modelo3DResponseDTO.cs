namespace GreenFil.Application.DTOs;

public class Modelo3DResponseDTO
{
    public int Id { get; set; }
    public string NombreModelo { get; set; } = null!;
    public DateTime? FechaCreacion { get; set; }
    public string RutaArchivo { get; set; } = null!;
    public string? ImagenPreview { get; set; }
    public string NombreUsuario { get; set; } = null!;
}