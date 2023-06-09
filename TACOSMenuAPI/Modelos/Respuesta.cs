namespace TACOSMenuAPI.Modelos;


/// <summary>
/// Estructura para responder a los clientes.
/// </summary>
/// <typeparam name="T">Tipo del cuerpo de la Respuesta; Respuesta<Alimento></typeparam>
public class Respuesta<T>
{
    /// <summary>
    /// Cuerpo de la respuesta, puede ser de cualquier tipo.
    /// </summary>
    public T? Datos { get; set; }

    /// <summary>
    /// Código HTTP de la respuesta. 500 por defecto.
    /// </summary>
    public int Codigo { get; set; } = 500;

    /// <summary>
    /// Mensaje de la respuesta.
    /// </summary>
    public string? Mensaje { get; set; } = string.Empty;
}
