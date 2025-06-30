namespace GreenFil.Domain.Entities;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombreproducto { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? Preciosoles { get; set; }

    public int? Preciopuntos { get; set; }

    public int? Stock { get; set; }

    public string? Imagen { get; set; }

    public virtual ICollection<Detallepedido> Detallepedidos { get; set; } = new List<Detallepedido>();
}
