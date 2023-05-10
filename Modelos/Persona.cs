﻿
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TACOS.Modelos;

public partial class Persona
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string ApellidoMaterno { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? Email { get; set; }

    public string Telefono { get; set; } = null!;

    [JsonIgnore]
    public ICollection<Miembro> Miembros { get; set; } = new List<Miembro>();
}
