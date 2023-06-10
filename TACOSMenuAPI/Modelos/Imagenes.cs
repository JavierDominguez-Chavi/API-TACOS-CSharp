using System;
using System.Collections.Generic;

namespace TACOSMenuAPI.Modelos;

/// <summary>
/// Imagen para alimentos.
/// </summary>
public partial class Imagen
{
    /// <summary>
    /// Llave primaria en la base de datos.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre descriptivo de la imagen.
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Representación en bytes de la imagen en la base de datos.
    /// </summary>
    public byte[]? ImagenBytes { get; set; }

    /// <summary>
    /// Alimentos que usan esta imagen.
    /// </summary>
    public List<Alimento>? Alimentos { get; set; }

}
