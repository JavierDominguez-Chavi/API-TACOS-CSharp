namespace TACOS.Negocio.PeticionesRespuestas;

/// <summary>
/// Rango de fechas para la consulta de Pedidos.
/// </summary>
public class RangoFecha
{
    /// <summary>
    /// Inicio del rango.
    /// </summary>
    public DateTime Desde { get; set; }

    /// <summary>
    /// Fin del rango.
    /// </summary>
    public DateTime Hasta { get; set; }

}
