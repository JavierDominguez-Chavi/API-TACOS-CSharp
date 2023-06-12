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
        /// <summary>
        /// Llave primaria en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Debe ser mayor a 8 caracteres, incluyendo: 1 minúscula, 1 mayúscula y un número.
        /// </summary>
        public string? Contrasena { get; set; }
        /// <summary>
        /// Llave foránea de la Persona a la que pertenece el Staff.
        /// </summary>
        public int IdPersona { get; set; }
        /// <summary>
        /// Llave foránea del Puesto que tiene el Staff.
        /// </summary>
        public int IdPuesto { get; set; }
        /// <summary>
        /// Llave foránea del Turno que tiene el Staff.
        /// </summary>
        public int IdTurno { get; set; }
        /// <summary>
        /// Persona a la que pertenece el Staff.
        /// </summary>
        public virtual Persona? Persona { get; set; } = new Persona();
        /// <summary>
        /// Puestos que tiene el Staff,
        /// </summary>
        [JsonIgnore]
        public virtual Puesto? Puesto { get; set; }
        /// <summary>
        /// Turnos que tiene el Staff,
        /// </summary>
        [JsonIgnore]
        public virtual Turno? Turno { get; set; } 
    }
}
