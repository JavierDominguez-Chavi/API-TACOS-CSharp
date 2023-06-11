#pragma warning disable CS1591
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TACOS.Modelos;
using TACOS.Modelos.Interfaces;
using System.Text.Json.Serialization;

namespace TACOS.Negocio.PeticionesRespuestas;

/// <summary>
/// Respuesta del inicio de sesión.
/// </summary>
public class RespuestaCredenciales
{

    /// <summary>
    /// Necesaria para simplificar la generación del token. NO serializar.
    /// </summary>
    [JsonIgnore]
    public IAsociado? Asociado { get; set; } = null;


    public Miembro? Miembro { get; set; } = null;
    public Staff? Staff { get; set; } = null;

    /// <summary>
    /// Codigo HTTP de la respuesta.
    /// </summary>
    public int Codigo { get; set; } = 500;

    public string? Mensaje { get; set; }

    /// <summary>
    /// Token de autenticación. Necesario para ciertas operaciones.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Tiempo de expiración del token.
    /// </summary>
    public int Expira { get; set; }
}