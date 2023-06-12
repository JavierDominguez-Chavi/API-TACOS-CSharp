#pragma warning disable CS1591
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TACOS.Modelos
{
    /// <summary>
    /// Turno en el que trabaja un determinado Staff.
    /// </summary>
    public class Turno
    {
        /// <summary>
        /// Llave primaria en la base de datos.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tipo de tuno.
        /// </summary>
        public string? Tipo { get; set; }
        /// <summary>
        /// Hora en la que inicia el turno dependiendo del Tipo
        /// </summary>
        [Column(TypeName = "time")]
        public TimeOnly? HoraInicio { get; set; }
        /// <summary>
        /// Hora en la que finaliza el turno dependiendo del Tipo
        /// </summary>
        [Column(TypeName = "time")]
        public TimeOnly? HoraFin { get; set; }
        /// <summary>
        /// Personas del Staff que tienen el Turno.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Staff>? Staff { get; set; } = new List<Staff>();
    }
}
