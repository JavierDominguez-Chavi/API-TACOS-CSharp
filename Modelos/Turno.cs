using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TACOS.Modelos
{
    public class Turno
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
        [Column(TypeName = "time")]
        public TimeOnly? HoraInicio { get; set; }
        [Column(TypeName = "time")]
        public TimeOnly? HoraFin { get; set; }

        [JsonIgnore]
        public virtual ICollection<Staff>? Staff { get; set; } = new List<Staff>();
    }
}
