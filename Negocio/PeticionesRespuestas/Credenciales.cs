using System.Text.Json.Serialization;
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

        /// <summary>
        /// Define si la sesión es para un Staff.
        /// </summary>
        public bool EsStaff { get; set; } = false;

        [JsonIgnore]
        public bool Llenas => 
            !String.IsNullOrWhiteSpace(this.Email) && !String.IsNullOrWhiteSpace(this.Contrasena);
    }
}
