﻿using System;
using System.Collections.Generic;

namespace TACOS.Modelos;

public partial class Categorium
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Medida { get; set; }

    public virtual ICollection<Alimento> Alimentos { get; set; } = new List<Alimento>();
}
