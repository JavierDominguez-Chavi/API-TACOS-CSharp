using System.Collections.ObjectModel;
using TACOS.Modelos;
using TACOS.Negocio.PeticionesRespuestas;

namespace TACOS.Negocio.Interfaces
{
    public interface IConsultanteMgt
    {
        public RespuestaCredenciales IniciarSesion(Credenciales credenciales);
        public Respuesta<Miembro> RegistrarMiembro(Miembro miembro);
        public Respuesta<Miembro> ConfirmarRegistro(Miembro miembro);
        public Respuesta<Pedido> RegistrarPedido(Pedido nuevoPedido);
        public Respuesta<List<PedidoReporte>> ObtenerPedidosEntre(RangoFecha rango);
        public Respuesta<Pedido> ActualizarPedido(PedidoSimple pedido);
        public List<Resena> ObtenerResenas();
        public bool BorrarResena(int idResena);
        public List<Puesto> ObtenerPuestos();
        public List<Turno> ObtenerTurnos();
        public void RegistrarEmpleadoStaff(Staff staff);
    }
}
