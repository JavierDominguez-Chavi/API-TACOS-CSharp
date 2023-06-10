using Newtonsoft.Json;

namespace TACOSMenuAPI.Modelos;

/// <summary>
/// Versión simplificada de Alimento, usada por ActualizarAlimentos.
/// </summary>
public class AlimentoActualizar
{
    /// <summary>
    /// Llave primaria en la base de datos.
    /// </summary>
    public int Id { get; set; }

    /// <summary></summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del alimento.
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Existencia actual. Debería ser mayor de 0.
    /// </summary>
    public int? Existencia { get; set; }

    /// <summary>
    /// Precio del alimento.
    /// </summary>
    public double? Precio { get; set; }

    /// <summary>
    /// Constructor vacío para el serializador.
    /// </summary>
    public AlimentoActualizar() { }

    /// <summary>
    /// Convierte un Alimento en AlimentoActualizar.
    /// </summary>
    /// <param name="alimento"></param>
    public AlimentoActualizar(Alimento alimento) 
    { 
        this.Id=alimento.Id;
        this.Nombre=alimento.Nombre;
        this.Descripcion=alimento.Descripcion;
        this.Existencia=alimento.Existencia;
        this.Precio=alimento.Precio;
    }
}
