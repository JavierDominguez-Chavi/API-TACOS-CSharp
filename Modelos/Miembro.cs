using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

/// <summary>
/// Credenciales de una Persona registrada en la base de datos.
/// </summary>
public partial class Miembro
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
    /// Número de pedidos que ha ordenado el individuo y que han transicionado
    /// al estado "Pagado"
    /// </summary>
    public int? PedidosPagados { get; set; }

    /// <summary>
    /// Llave foránea de la Persona a la que pertenece el Miembro.
    /// </summary>
    public int IdPersona { get; set; }

    /// <summary>
    /// Código para confirmar registro.
    /// </summary>
    public int? CodigoConfirmacion { get; set; }

    /// <summary>
    /// Persona a la que pertenece el Miembro.
    /// </summary>
    public virtual Persona? Persona { get; set; }

    /// <summary>
    /// Pedidos del miembro.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Pedido>? Pedidos { get; set; } = new List<Pedido>();

    /// <summary>
    /// Reseñas del Miembro.
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Resena>? Resenas { get; set; } = new List<Resena>();
}
