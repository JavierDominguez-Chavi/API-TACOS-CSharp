#pragma warning disable CS1591
using System.Text.Json.Serialization;
using TACOS.Modelos.Interfaces;

namespace TACOS.Modelos
{
    /// <summary>
    /// Credenciales de una Persona empleada en la base de datos.
    /// </summary>
    public class Staff : IAsociado
    {
        public int Id { get; set; }
        public string? Contrasena { get; set; }
        public int IdPersona { get; set; }
        public int IdPuesto { get; set; }
        public int IdTurno { get; set; }
        public virtual Persona? Persona { get; set; } = new Persona();

        [JsonIgnore]
        public virtual Puesto? Puesto { get; set; }
        [JsonIgnore]
        public virtual Turno? Turno { get; set; } 
    }
}
