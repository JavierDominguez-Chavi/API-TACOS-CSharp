using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACOS.Modelos
{
    public class Resena
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }

        [Column(TypeName = "mediumblob")]
        public byte[]? Imagen { get; set; }

        public DateTime Fecha { get; set; }

        public int IdMiembro { get; set; }

        public virtual Miembro Miembro { get; set; } = new Miembro();
    }
}
