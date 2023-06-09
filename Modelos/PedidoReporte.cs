namespace TACOS.Modelos;
using System.Text.Json.Serialization;

/// <summary>
/// Versión de Pedido exclusiva para la generación de reportes.
/// </summary>
public class PedidoReporte
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
    public int? Estado { get; set; }

    /// <summary>
    /// Fecha y hora en la que se realizó el Pedido por el Miembro.
    /// </summary>
    public DateTime? Fecha { get; set; }

    /// <summary>
    /// Alimentos en el Pedido.
    /// </summary>
    public ICollection<AlimentosPedidosReporte>? Alimentospedidos { get; set; } =
    new List<AlimentosPedidosReporte>();

    /// <summary>
    /// Miembro del Pedido.
    /// </summary>
    public Miembro? Miembro { get; set; }

    /// <summary>
    /// Convierte Pedidos a PedidoReportes.
    /// </summary>
    /// <param name="pedido"></param>
    public PedidoReporte(Pedido pedido)
    { 
        this.Id = pedido.Id;
        this.IdMiembro = pedido.IdMiembro;
        this.Fecha = pedido.Fecha;
        this.Total = pedido.Total;
        this.Estado = pedido.Estado;
        if (pedido.Miembro != null)
        {
            this.Miembro = pedido.Miembro!;
        }
        foreach (Alimentospedido alimentoPedido in pedido.Alimentospedidos)
        {
            this.Alimentospedidos.Add(
                new AlimentosPedidosReporte { 
                    Alimento = alimentoPedido.Alimento,
                    IdAlimento = alimentoPedido.IdAlimento,
                    IdPedido = alimentoPedido.IdPedido,
                    Cantidad = alimentoPedido.Cantidad
                });
        }
    }
}
