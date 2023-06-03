namespace TACOS.Modelos;
using System.Text.Json.Serialization;

public class PedidoReporte
{
    public int Id { get; set; }

    public double? Total { get; set; }

    public int IdMiembro { get; set; }

    public int? Estado { get; set; }

    public DateTime? Fecha { get; set; }
    public new ICollection<AlimentosPedidosReporte> Alimentospedidos { get; set; } =
    new List<AlimentosPedidosReporte>();

    public Miembro Miembro { get; set; }

    public PedidoReporte(Pedido pedido)
    { 
        this.Id = pedido.Id;
        this.IdMiembro = pedido.IdMiembro;
        this.Fecha = pedido.Fecha;
        this.Total = pedido.Total;
        this.Estado = pedido.Estado;
        this.Miembro = pedido.Miembro;
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
