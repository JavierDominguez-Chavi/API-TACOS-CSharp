using Microsoft.AspNetCore.Mvc;
using TACOS.Modelos;
using TACOS.Negocio.Interfaces;
using TACOS.Negocio;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TACOS.Negocio.PeticionesRespuestas;

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

        [HttpPost(Name = "RegistrarPedido")]
        [Authorize]
        public IActionResult RegistrarPedido([FromBody] Pedido pedido)
        {
            try
            {
                return new JsonResult(_consultanteMgr.RegistrarPedido(pedido));
            }
            catch (Exception exception)
            {
                return new JsonResult(new Error { Mensaje = exception.Message }) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Obtiene pedidos por rango de fechas.
        /// </summary>
        /// <remarks>
        /// Regresa una lista de PedidoSimple (Pedido sin propiedad Miembro)
        /// </remarks>
        /// <response code="200">La petición fue aceptada.</response>
        /// <response code="401">El código es incorrecto.</response>
        /// <response code="404">No se encontró el miembro solicitado.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Miembro con el código de confirmación limpio.</returns>
        /// <param name="rango"></param>
        [ProducesResponseType(typeof(List<PedidoSimple>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpPost("ObtenerPedidosEntre")]
        //[Authorize]
        public IActionResult ObtenerPedidosEntre([FromBody] RangoFecha rango)
        {
            try
            {
                return new JsonResult(this._consultanteMgr.ObtenerPedidosEntre(rango)) { StatusCode = 200 };
            }
            catch (HttpRequestException httpRequestException)
            {
                string mensaje = "";
                switch (httpRequestException.Message)
                {
                    case "404":
                        mensaje = $"No se encontraron pedidos entre {rango.Desde} y {rango.Hasta}.";
                        break;
                    default:
                        mensaje = "No hay conexión con la base de datos.";
                        break;
                }
                return new JsonResult(
                    new { mensaje }
                ){ StatusCode = Int32.Parse(httpRequestException.Message) };
            }
        }

        [HttpPatch(Name = "ActualizarPedido")]
        [Authorize]
        public IActionResult ActualizarPedido(PedidoSimple pedido)
        {
            try
            {
                this._consultanteMgr.ActualizarPedido(pedido);
                return new JsonResult(pedido) { StatusCode = 200 };
            }
            catch (HttpRequestException httpRequestException)
            {
                string mensaje = "";
                switch (httpRequestException.Message)
                {
                    case "404":
                        mensaje = "No se encontró el pedido y/o miembro solicitado.";
                        break;
                    case "400":
                        mensaje = "Todos los campos son obligatorios.";
                        break;
                    case "403":
                        mensaje = "El estado del pedido ya no puede cambiar.";
                        break;
                    case "422":
                        mensaje = "El pedido solicitado pertenece a un miembro distinto.";
                        break;
                    default:
                        mensaje = "No hay conexión con la base de datos.";
                        break;
                }
                return new JsonResult(
                    new { mensaje }
                ){ StatusCode = Int32.Parse(httpRequestException.Message) };
            }
        }
    }
}
