using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

public class PedidoSimple : Pedido 
{
    [JsonIgnore]
    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();
    [JsonIgnore]
    public virtual Miembro? Miembro { get; set; } = null!;

}
