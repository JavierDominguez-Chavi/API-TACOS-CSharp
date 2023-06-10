using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

/// <summary>
/// Relación entre Alimentos y Pedidos.
/// </summary>
public partial class Alimentospedido
{
    /// <summary>
    /// Llave primaria en la base de datos.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Cantidad ordenada del IdAlimento. Debería ser mayor a 0.
    /// </summary>
    public int? Cantidad { get; set; }

    /// <summary>
    /// Llave foránea del alimento que se está pidiendo.
    /// </summary>
    public int IdAlimento { get; set; }

    /// <summary>
    /// Llave foránea del pedido al que pertenece.
    /// </summary>
    public int IdPedido { get; set; }

    /// <summary>
    /// Alimento que se está pidiendo.
    /// </summary>
    [JsonIgnore]
    public virtual Alimento? Alimento { get; set; } = null!;

    /// <summary>
    /// Pedido al que pertenece.
    /// </summary>
    [JsonIgnore]
    public virtual Pedido? Pedido { get; set; } = null!;
}

