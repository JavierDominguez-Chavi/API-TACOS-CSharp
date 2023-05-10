using TACOS.Modelos;

namespace TACOS.Negocio.Interfaces
{
    public interface IMenuMgt
    {
        public List<Alimento> ObtenerAlimentos();
        public void RegistrarPedido();
    }
}
