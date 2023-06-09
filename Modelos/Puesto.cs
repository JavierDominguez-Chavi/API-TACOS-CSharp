﻿#pragma warning disable CS1591
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
        /// <summary>
        /// Llave primaria en la base de datos.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Tipo de cargo.
        /// </summary>
        public string? Cargo { get; set; }
        /// <summary>
        /// Personas del Staff que tienen el Puesto
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Staff>? Staff { get; set; } = new List<Staff>();
    }
}
