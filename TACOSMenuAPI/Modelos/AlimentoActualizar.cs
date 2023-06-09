using Newtonsoft.Json;

namespace TACOSMenuAPI.Modelos;

public class AlimentoActualizar
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Existencia { get; set; }

    public double? Precio { get; set; }

    public AlimentoActualizar() { }
    public AlimentoActualizar(Alimento alimento) 
    { 
        this.Id=alimento.Id;
        this.Nombre=alimento.Nombre;
        this.Descripcion=alimento.Descripcion;
        this.Existencia=alimento.Existencia;
        this.Precio=alimento.Precio;
    }
}
