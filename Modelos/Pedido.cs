using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

public partial class Pedido
{
    public int Id { get; set; }

    public double? Total { get; set; }

    public int IdMiembro { get; set; }

    [Range(0,3, ErrorMessage ="El identificador del estado debe estar entre 0 y 4.")]
    public int? Estado { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();

    [JsonIgnore]
    public virtual Miembro? Miembro { get; set; } = null!;
}
