namespace GreenFil.Domain.Entities;

public partial class Modelospremium
{
    public int Id { get; set; }
    public string NombreModelo { get; set; } = null!;
    public string? Descripcion { get; set; }
    public decimal PrecioSoles { get; set; } = 0.00M;
    public int PrecioPuntos { get; set; } = 0;
    public string RutaGLB { get; set; } = null!;
    public string RutaSTL { get; set; } = null!;
    public string? RutaPreview { get; set; }
    public string Estado { get; set; } = "pendiente";
    public DateTime FechaRegistro { get; set; }
}