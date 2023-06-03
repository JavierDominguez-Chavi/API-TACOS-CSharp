using System.Collections.ObjectModel;
using TACOS.Modelos;
using TACOS.Negocio.PeticionesRespuestas;

namespace TACOS.Negocio.Interfaces
{
    public interface IConsultanteMgt
    {
        public Miembro IniciarSesion(Credenciales credenciales);
        public bool RegistrarMiembro(Miembro miembro);
        public bool ConfirmarRegistro(Miembro miembro);
        public bool RegistrarPedido(Pedido nuevoPedido);
        public List<PedidoReporte> ObtenerPedidosEntre(RangoFecha rango);
        public bool ActualizarPedido(PedidoSimple pedido);
        public List<Resena> ObtenerResenas();
    }
}
