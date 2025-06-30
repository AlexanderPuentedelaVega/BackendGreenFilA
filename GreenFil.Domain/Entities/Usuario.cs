using System;
using System.Collections.Generic;

namespace GreenFil.Domain.Entities;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombreusuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public int? Puntos { get; set; }

    public DateTime? Fecharegistro { get; set; }

    public string? Rol { get; set; }

    public virtual ICollection<Modelo3d> Modelo3ds { get; set; } = new List<Modelo3d>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
