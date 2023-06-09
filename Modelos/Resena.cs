using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace TACOS.Modelos
{
    /// <summary>
    /// Reseña hecha por un Miembro.
    /// </summary>
    public class Resena
    {
        /// <summary>
        /// Llave primaria en la base de datos.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Comentarios hechos por el Miembro.
        /// </summary>
        public string? Descripcion { get; set; }

        /// <summary>
        /// Imagen que acompaña a la reseña.
        /// </summary>
        [Column(TypeName = "mediumblob")]
        public byte[]? Imagen { get; set; }

        /// <summary>
        /// Fecha y hora en que fue publicada la reseña.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Llave foránea del Miembro responsable por escribir la Reseña.
        /// </summary>
        public int IdMiembro { get; set; }

        /// <summary>
        ///  Miembro responsable por escribir la Reseña.
        /// </summary>
        public virtual Miembro Miembro { get; set; } = new Miembro();
    }
}
