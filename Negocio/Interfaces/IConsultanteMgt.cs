using System.Collections.ObjectModel;
using TACOS.Modelos;
using TACOS.Negocio.PeticionesRespuestas;

namespace TACOS.Negocio.Interfaces
{
    /// <summary>
    /// Interfaz responsable de las operaciones relacionadas con los usuarios de TACOS.
    /// </summary>
    public interface IConsultanteMgt
    {
        /// <summary>
        /// Permite acceder a las funcionalidades de TACOS.
        /// </summary>
        /// <param name="credenciales">email y contraseña de un miembro registrado.</param>
        /// <returns>El Miembro al que le pertenecen las Credenciales provistas.</returns>
        public RespuestaCredenciales IniciarSesion(Credenciales credenciales);

        /// <summary>
        /// Agrega un nuevo Miembro a la base de datos.
        /// </summary>
        /// <param name="miembro">Miembro y Persona nuevos.</param>
        /// <returns>El Miembro, con la información de identificación en la base de datos asignada
        /// , y con el código de confirmación asignado.</returns>
        public Respuesta<Miembro> RegistrarMiembro(Miembro miembro);

        /// <summary>
        /// Confirma el registro de un Miembro nuevo.
        /// </summary>
        /// <param name="miembro">Requiere su Id y el CodigoConfirmacion</param>
        /// <returns>El mismo Miembro, pero con el CodigoConfirmacion fijado en 0.</returns>
        public Respuesta<Miembro> ConfirmarRegistro(Miembro miembro);

        /// <summary>
        /// Agrega un Pedido a la base de datos.
        /// </summary>
        /// <param name="nuevoPedido">Pedido relacionado al Miembro que lo hizo, y los Alimentos que pidió.</param>
        /// <returns>Pedido registrado con su información de base de datos.</returns>
        public Respuesta<Pedido> RegistrarPedido(Pedido nuevoPedido);

        /// <summary>
        /// Devuelve Pedidos con sus Miembros y Alimentos respectivos, que se hayan realizado en un rango especificado.
        /// </summary>
        /// <param name="rango">Rango en el que se quiere consultar los Pedidos.</param>
        /// <returns>Los Pedidos hechos en el rango especificado.</returns>
        public Respuesta<List<PedidoReporte>> ObtenerPedidosEntre(RangoFecha rango);

        /// <summary>
        /// Actualiza un Pedido. Se usa sólo para cambiar los estados, aunque puede ser extendido.
        /// </summary>
        /// <param name="pedido">El Pedido a modificar. Necesita su Id y el nuevo Estado.</param>
        /// <returns>Pedido modificado.</returns>
        public Respuesta<Pedido> ActualizarPedido(PedidoSimple pedido);

        /// <summary>
        /// Consulta las reseñas.
        /// </summary>
        /// <returns>Todas las reseñas en la base de datos.</returns>
        public List<Resena> ObtenerResenas();
    }
}
