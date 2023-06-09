namespace TACOS.Negocio.PeticionesRespuestas
{
    /// <summary>
    /// Información para iniciar sesión.
    /// </summary>
    public class Credenciales
    {
        /// <summary>
        /// Correo electrónico de la persona que quiere iniciar sesión.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Contraseña de la persona.
        /// </summary>
        public string? Contrasena { get; set; }
    }
}
