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
    /// <summary>
    /// Llave primaria en la base de datos.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del alimento.
    /// </summary>
    public string? Nombre { get; set; }

    /// <summary>
    /// Descripción del alimento.
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Existencia actual. Debería ser mayor de 0.
    /// </summary>
    public int? Existencia { get; set; }

    /// <summary>
    /// Llave foránea de la imágen que utiliza este alimento para desplegarse en el menú.
    /// </summary>
    public int IdImagen { get; set; }

    /// <summary>
    /// Contiene la imágen del alimento.
    /// </summary>
    public Imagen? Imagen { get; set; }

    /// <summary>
    /// Precio del alimento.
    /// </summary>
    public double? Precio { get; set; }

    /// <summary>
    /// Llave foránea de la categoría a la cual pertenece.
    /// </summary>
    public int IdCategoria { get; set; }

    /// <summary>
    /// Telación con Pedidos.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();

    /// <summary>
    /// Categoría a la que pertenece.
    /// </summary>
    [JsonIgnore]
    public virtual Categorium Categoria { get; set; } = null!;
}
