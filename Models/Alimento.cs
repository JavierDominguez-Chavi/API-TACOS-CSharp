﻿using System;
using System.Collections.Generic;

namespace TACOS.Models;

public partial class Alimento
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Existencia { get; set; }

    public byte[]? Imagen { get; set; }

    public double? Precio { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int IdCategoria { get; set; }

    public virtual ICollection<Alimentospedido> Alimentospedidos { get; set; } = new List<Alimentospedido>();

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;
}
