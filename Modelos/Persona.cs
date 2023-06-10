﻿using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;

namespace TACOS.Modelos;

public partial class Persona : ICloneable
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Email { get; set; }

    public string Telefono { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Miembro> Miembros { get; set; } = new List<Miembro>();

    [JsonIgnore]
    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
