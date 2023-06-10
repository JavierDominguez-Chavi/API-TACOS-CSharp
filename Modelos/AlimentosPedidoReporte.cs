namespace TACOS.Modelos;

/// <summary>
/// Utilizada exclusivamente para la generación de reportes.
/// </summary>
public class AlimentosPedidosReporte
{
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
    /// Alimento que se se pidió.
    /// </summary>
    public Alimento? Alimento { get; set; } = null!;
}
