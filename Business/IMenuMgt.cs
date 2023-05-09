using TACOS.Models;

namespace TACOS.Business
{
    public interface IMenuMgt
    {
        public List<Alimento> ObtenerAlimentos();
        public void RegistrarPedido();
    }
}
