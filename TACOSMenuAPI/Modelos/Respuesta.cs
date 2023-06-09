namespace TACOSMenuAPI.Modelos;

public class Respuesta<T>
{
    public T Datos { get; set; }
    public int Codigo { get; set; } = 500;
    public string Mensaje { get; set; } = string.Empty;
}
