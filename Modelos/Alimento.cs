using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;
namespace TACOS.Modelos;

/// <summary>
/// Alimento
/// </summary>
public partial class Alimento
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Existencia { get; set; }

    public int IdImagen { get; set; }

    public Imagen Imagen { get; set; }

    public double? Precio { get; set; }

    public int IdCategoria { get; set; }

    [JsonIgnore]
    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();
    [JsonIgnore]
    public virtual Categorium Categoria { get; set; } = null!;
}
