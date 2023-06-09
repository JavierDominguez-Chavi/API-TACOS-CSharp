using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

/// <summary>
/// Pedidos hechos por Miembros.
/// </summary>
public partial class Pedido
{
    /// <summary>
    /// Llave primaria en la base de datos.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sumatoria de los costos de cada AlimentoPedido.
    /// </summary>
    public double? Total { get; set; }

    /// <summary>
    /// Llave foránea del Miembro al cual pertenece el Pedido.
    /// </summary>
    public int IdMiembro { get; set; }

    /// <summary>
    /// Estado del pedido; 0 (Cancelado), 1 (Pedido), 2 (Enviado) y 3 (Pagado).
    /// El Estado no puede cambiar una vez que llega al 3.
    /// </summary>
    [Range(0,3, ErrorMessage ="El identificador del estado debe estar entre 0 y 4.")]
    public int? Estado { get; set; }

    /// <summary>
    /// Fecha y hora en la que se realizó el Pedido por el Miembro.
    /// </summary>
    public DateTime? Fecha { get; set; }

    /// <summary>
    /// Alimentos en el Pedido.
    /// </summary>
    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();

    /// <summary>
    /// Miembro del Pedido.
    /// </summary>
    [JsonIgnore]
    public virtual Miembro? Miembro { get; set; } = null!;
}
