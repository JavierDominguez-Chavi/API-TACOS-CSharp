#pragma warning disable CS1591
namespace TACOS.Modelos.Interfaces
{
    /// <summary>
    /// Unifica las clases Miembros y Staff, asociados a Persona, 
    /// para simplificar operaciones.
    /// </summary>
    public interface IAsociado
    {
        public string? Contrasena { get; set; }
        public Persona? Persona { get; set; }
    }
}
