namespace GreenFil.Domain.Entities;

public partial class Pedido
{
    public int Id { get; set; }

    public int Usuarioid { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal? Totalsoles { get; set; }

    public int? Totalpuntos { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Detallepedido> Detallepedidos { get; set; } = new List<Detallepedido>();

    public virtual Usuario Usuario { get; set; } = null!;
}