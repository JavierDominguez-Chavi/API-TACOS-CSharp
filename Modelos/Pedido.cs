using System;
using System.Collections.Generic;

namespace TACOS.Modelos;

public partial class Pedido
{
    public int Id { get; set; }

    public double? Total { get; set; }

    public DateTime Fecha { get; set; }

    public int IdMiembro { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();

    public virtual Miembro? IdMiembroNavigation { get; set; } = null!;
}
