using System;
using System.Collections.Generic;

namespace TACOSMenuAPI.Modelos;

public partial class Imagen
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public byte[]? ImagenBytes { get; set; }
    public List<Alimento> Alimentos { get; set; }

}
