namespace TACOS.Modelos;

public class AlimentosPedidosReporte
{
    public int? Cantidad { get; set; }

    public int IdAlimento { get; set; }

    public int IdPedido { get; set; }
    public Alimento? Alimento { get; set; } = null!;
}
