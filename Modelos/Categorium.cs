using System;
using System.Collections.Generic;

namespace TACOS.Modelos;

/// <summary>
/// Categoría de alimentos. No se usa por el momento.
/// </summary>
public partial class Categorium
{
    /// <summary>
    /// Llave primaria en la base de datos.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Ejemplo: bebida
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Ejemplos: kg, gr, ml
    /// </summary>
    public string? Medida { get; set; }

    /// <summary>
    /// Alimentos que pertenecen a esta categoría.
    /// </summary>
    public virtual ICollection<Alimento> Alimentos { get; set; } = new List<Alimento>();
}
