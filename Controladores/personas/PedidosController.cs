using Microsoft.AspNetCore.Mvc;
using TACOS.Modelos;
using TACOS.Negocio.Interfaces;
using TACOS.Negocio;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("personas/[controller]")]
    public class PedidosController : Controller
    {

        private readonly ILogger<PedidosController> logger;
        private IMenuMgt _menuMgr;

        public PedidosController(ILogger<PedidosController> logger,
                                  IMenuMgt menuMgr)
        {
            this.logger = logger;
            _menuMgr = menuMgr;
        }

        [HttpPost(Name = "RegistrarPedido")]
        public IActionResult RegistrarPedido([FromBody] Pedido pedido)
        {
            try
            {
                return new JsonResult(_menuMgr.RegistrarPedido(pedido));
            }
            catch (Exception exception)
            {
                return new JsonResult(new Error { Mensaje = exception.Message}) { StatusCode = 500};
            }
            
        }
    }
}
