using System;
using System.Collections.Generic;

namespace GreenFil.Domain.Entities;

public partial class Detallepedido
{
    public int Id { get; set; }

    public int Pedidoid { get; set; }

    public int Productoid { get; set; }

    public int Cantidad { get; set; }

    public decimal? Subtotalsoles { get; set; }

    public int? Subtotalpuntos { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
