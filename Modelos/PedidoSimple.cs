using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

/// <summary>
/// Versión de simplificada de Pedido para ciertas operaciones.
/// </summary>
public class PedidoSimple : Pedido
{
    /// <summary>
    /// Alimentos en el Pedido.
    /// </summary>
    [JsonIgnore]
    public virtual new ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();
    /// <summary>
    /// Miembro del Pedido.
    /// </summary>
    [JsonIgnore]
    public virtual new Miembro? Miembro { get; set; } = null!;

    /// <summary>
    /// Constructor vacío para serializar.
    /// </summary>
    public PedidoSimple() { }

    /// <summary>
    /// Constructor para convertir un Pedido a PedidoSimple.
    /// </summary>
    public PedidoSimple(Pedido pedidoModelo)
    {
        this.Id = pedidoModelo.Id;
        this.Total = pedidoModelo.Total;
        this.Fecha = pedidoModelo.Fecha;
        this.IdMiembro = pedidoModelo.IdMiembro;
        this.Estado = pedidoModelo.Estado;
    }

}