using System.ComponentModel;
using System.Runtime.CompilerServices;
using TACOS.Modelos;

namespace TACOS.Negocio.PeticionesRespuestas;

/// <summary>
/// Respuesta del inicio de sesión.
/// </summary>
public class RespuestaCredenciales
{
    /// <summary>
    /// Miembro obtenido, cuyas credenciales coinciden con las provistas a IniciarSesion().
    /// </summary>
    public Miembro? Miembro { get; set; } = null;

    /// <summary>
    /// Codigo HTTP de la respuesta.
    /// </summary>
    public int Codigo { get; set; } = 500;

    /// <summary>
    /// Descripción de la respuesta.
    /// </summary>
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