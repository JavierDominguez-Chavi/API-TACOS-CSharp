namespace TACOS.Modelos.Interfaces
{
    public interface IAsociado
    {
        public string Contrasena { get; set; }
        public Persona Persona { get; set; }
    }
}
