using System.Collections.ObjectModel;
using TACOS.Modelos;

namespace TACOS.Negocio.Interfaces
{
    public interface IConsultanteMgt
    {
        public Persona IniciarSesion(Credenciales credenciales);
        public bool RegistrarMiembro(Persona persona);
        public bool ConfirmarRegistro(Persona persona);
        public bool RegistrarPedido(Pedido nuevoPedido);
        public List<Pedido> ObtenerPedidos();
        public bool ActualizarPedido(PedidoSimple pedido);
        public List<Resena> ObtenerResenas();
    }
}
