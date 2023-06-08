using Microsoft.AspNetCore.Mvc;
using TACOS.Modelos;
using TACOS.Negocio.Interfaces;
using TACOS.Negocio;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TACOS.Negocio.PeticionesRespuestas;
using System.Net.Http;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController : Controller
    {

        private readonly ILogger<PedidosController> logger;
        private IConsultanteMgt _consultanteMgr;

        public PedidosController(ILogger<PedidosController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            _consultanteMgr = consultanteMgr;
        }

        /// <summary>
        /// RegistrarPedido(). Registra un Pedido que contenga Alimentopedidos.
        /// </summary>
        /// <remarks>
        /// Si el registro es exitoso, se asigna un CodigoConfirmacion al Miembro y 
        /// se envía al Email proporcionado.
        /// </remarks>
        /// <response code="200">Operación exitosa.</response>
        /// <response code="400">Ya existe una cuenta con este email.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="500">Error en el servidor.</response>
        /// <returns>Miembro con el código de confirmación limpio.</returns>
        [ProducesResponseType(typeof(Respuesta<Pedido>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<Pedido>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Respuesta<Pedido>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost(Name = "RegistrarPedido")]
        [Authorize]
        public IActionResult RegistrarPedido([FromBody] Pedido pedido)
        {
            Respuesta<Pedido> respuesta = _consultanteMgr.RegistrarPedido(pedido);
            return new JsonResult(respuesta) { StatusCode = respuesta.Codigo};
        }

        /// <summary>
        /// Obtiene pedidos por rango de fechas.
        /// </summary>
        /// <remarks>
        /// Regresa una lista de PedidoSimple (Pedido sin propiedad Miembro)
        /// </remarks>
        /// <response code="200">La petición fue aceptada.</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="404">No se encontraron pedidos en el rango especificado.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Miembro con el código de confirmación limpio.</returns>
        /// <param name="rango"></param>
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpPost("ObtenerPedidosEntre")]
        [Authorize]
        public IActionResult ObtenerPedidosEntre([FromBody] RangoFecha rango)
        {
            var respuesta = this._consultanteMgr.ObtenerPedidosEntre(rango);
            return new JsonResult(respuesta) { StatusCode = respuesta.Codigo };
        }

        /// <summary>
        /// ActualizarPedido(). Modifica un pedido existente.
        /// </summary>
        /// <remarks>
        /// Regresa el pedido con los nuevos valores asignados. Id e IdMiembro son obligatorios.
        /// </remarks>
        /// <response code="200">La petición fue aceptada.</response>
        /// <response code="400">Id vacío; IdMiembro vacío; Estado fuera del rango [0,3]</response>
        /// <response code="401">No autorizado.</response>
        /// <response code="403">El pedido no puede cambiar su estado, pues ya fue pagado.</response>
        /// <response code="404">El pedido solicitado no existe.</response>
        /// <response code="422">El pedido solicitado pertenece a un miembro distinto al especificado.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Pedido con los nuevos valores asignados.</returns>
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Respuesta<List<PedidoSimple>>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPatch(Name = "ActualizarPedido")]
        //[Authorize]
        public IActionResult ActualizarPedido(PedidoSimple pedido)
        {
            var respuesta = this._consultanteMgr.ActualizarPedido(pedido);
            return new JsonResult(respuesta) { StatusCode = respuesta.Codigo };
        }
    }
}
