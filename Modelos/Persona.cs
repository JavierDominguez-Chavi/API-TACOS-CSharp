using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace TACOS.Modelos;

/// <summary>
/// Persona registrada en la base de datos.
/// </summary>
public partial class Persona
{
    /// <summary>
    /// Llave primaria en la base de datos.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre(s) propio. Debe ser alfanumérico.
    /// </summary>
    public string Nombre { get; set; } = null!;

    /// <summary>
    /// Apellido paterno. Debe ser alfanumérico.
    /// </summary>
    public string ApellidoPaterno { get; set; } = null!;

    /// <summary>
    /// Apellido materno. Debe ser alfanumérico.
    /// </summary>
    public string ApellidoMaterno { get; set; } = null!;

    /// <summary>
    /// Domicilio de la Persona.
    /// </summary>
    public string Direccion { get; set; } = null!;

    /// <summary>
    /// Correo electrónico. Debe seguir un formato válido.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Debe seguir un formato válido.
    /// </summary>
    public string Telefono { get; set; } = null!;

    /// <summary>
    /// Miembros relacionados con la Persona. Debería siempre ser un sólo Miembro.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Miembro> Miembros { get; set; } = new List<Miembro>();
}
