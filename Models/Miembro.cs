using System;
using System.Collections.Generic;

namespace TACOS.Models;

public partial class Miembro
{
    public int Id { get; set; }

    public string? Contrasena { get; set; }

    public int? PedidosPagados { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int IdPersona { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
