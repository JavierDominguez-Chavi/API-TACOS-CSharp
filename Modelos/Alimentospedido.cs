using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

public partial class Alimentospedido
{
    public int Id { get; set; }

    public int? Cantidad { get; set; }

    public int IdAlimento { get; set; }

    public int IdPedido { get; set; }

    [JsonIgnore]
    public virtual Alimento? Alimento { get; set; } = null!;
    [JsonIgnore]
    public virtual Pedido? Pedido { get; set; } = null!;
}

