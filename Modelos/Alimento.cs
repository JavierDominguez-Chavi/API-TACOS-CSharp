using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TACOS.Modelos;

public partial class Alimento
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Existencia { get; set; }

    public byte[]? Imagen { get; set; }

    public double? Precio { get; set; }

    public int IdCategoria { get; set; }

    [JsonIgnore]
    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();

    [JsonIgnore]
    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;
    

}
