using TACOS.Modelos;

namespace TACOS.Negocio.Interfaces
{
    public interface IConsultanteMgt
    {
        public Persona IniciarSesion(Persona credenciales);
        public bool RegistrarMiembro(Persona persona);
    }
}
