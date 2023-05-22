using Microsoft.AspNetCore.Mvc;
using TACOS.Modelos;
using TACOS.Negocio.Interfaces;
using TACOS.Negocio;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet(Name = "ObtenerPedidos")]
        public IActionResult ObtenerPedidos()
        {
            try
            {
                return new JsonResult(_consultanteMgr.ObtenerPedidos()) { StatusCode = 200 };
            }
            catch (Exception exception)
            {
                return new JsonResult(
                    new Error { Mensaje = "No hay conexión con la base de datos" }
                )
                { StatusCode = 500 };
            }
        }

        [HttpPatch(Name = "ActualizarPedido")]
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
