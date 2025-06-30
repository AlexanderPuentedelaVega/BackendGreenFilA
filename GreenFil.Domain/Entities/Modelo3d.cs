using System;
using System.Collections.Generic;

namespace GreenFil.Domain.Entities;

public partial class Modelo3d
{
    public int Id { get; set; }

    public int Usuarioid { get; set; }

    public string Nombremodelo { get; set; } = null!;

    public DateTime? Fechacreacion { get; set; }

    public string Rutaarchivo { get; set; } = null!;

    public string? Imagenpreview { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
