using System.Collections.ObjectModel;
using TACOS.Modelos;

namespace TACOS.Negocio.Interfaces
{
    public interface IConsultanteMgt
    {
        public Persona IniciarSesion(Persona credenciales);
        public bool RegistrarMiembro(Persona persona);
        public bool ConfirmarRegistro(Persona persona);
        public bool RegistrarPedido(Pedido nuevoPedido);
        public ObservableCollection<Pedido> ObtenerPedidos();
        public bool ActualizarPedido(Pedido pedido);
        public List<Resena> ObtenerResenas();
    }
}
