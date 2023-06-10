using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TACOS.Modelos.Interfaces;

namespace TACOS.Modelos;

public partial class Miembro : IAsociado
{
    public int Id { get; set; }

    public string? Contrasena { get; set; }

    public int? PedidosPagados { get; set; }

    public int IdPersona { get; set; }

    public int? CodigoConfirmacion { get; set; }

    public virtual Persona? Persona { get; set; }
    [JsonIgnore]
    public virtual ICollection<Pedido>? Pedidos { get; set; } = new List<Pedido>();
    [JsonIgnore]
    public virtual ICollection<Resena>? Resenas { get; set; } = new List<Resena>();
}
