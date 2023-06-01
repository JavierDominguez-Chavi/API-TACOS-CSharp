using System.Collections.ObjectModel;
using TACOS.Modelos;
using TACOS.Modelos.PeticionesRespuestas;

namespace TACOS.Negocio.Interfaces
{
    public interface IConsultanteMgt
    {
        public Persona IniciarSesion(Credenciales credenciales);
        public bool RegistrarMiembro(Miembro miembro);
        public bool ConfirmarRegistro(Miembro miembro);
        public bool RegistrarPedido(Pedido nuevoPedido);
        public List<Pedido> ObtenerPedidos();
        public bool ActualizarPedido(PedidoSimple pedido);
        public List<Resena> ObtenerResenas();
    }
}
