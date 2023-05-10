using TACOS.Modelos;

namespace TACOS.Negocio.Interfaces
{
    public interface IMenuMgt
    {
        public List<Alimento> ObtenerAlimentos();
        public int ActualizarExistencia(Alimentospedido alimento);
        public bool RegistrarPedido(Pedido nuevoPedido);
    }
}
