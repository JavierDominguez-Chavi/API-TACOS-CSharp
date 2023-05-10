using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio.Interfaces;

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

        [HttpPost(Name = "PostPedido")]
        public void RegistrarPedido()
        {
            _menuMgr.RegistrarPedido();
        }
    }
}
