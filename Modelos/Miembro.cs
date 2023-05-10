using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

public partial class Miembro
{
    public int Id { get; set; }

    public string? Contrasena { get; set; }

    public int? PedidosPagados { get; set; }

    public int IdPersona { get; set; }

    public Persona Persona { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
