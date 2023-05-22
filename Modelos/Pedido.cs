using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

public partial class Pedido
{
    public int Id { get; set; }

    public double? Total { get; set; }

    public int IdMiembro { get; set; }

    public int? Estado { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();

    public virtual Miembro? Miembro { get; set; } = null!;
}
