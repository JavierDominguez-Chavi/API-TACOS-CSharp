#pragma warning disable CS1591
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TACOS.Modelos
{
    /// <summary>
    /// Tipos de contratación en el establecimiento.
    /// </summary>
    public class Puesto
    {
        public int Id { get; set; }

        public string? Cargo { get; set; }

        [JsonIgnore]
        public virtual ICollection<Staff>? Staff { get; set; } = new List<Staff>();
    }
}
