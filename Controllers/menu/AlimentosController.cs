namespace TACOS.Controllers.menu
{
    using Microsoft.AspNetCore.Mvc;
    using TACOS.Business;

    [ApiController]
    [Route("menu/[controller]")]
    public class AlimentosController : ControllerBase
    {
        private readonly ILogger<AlimentosController> logger;
        private IMenuMgt _menuMgr;

        public AlimentosController(ILogger<AlimentosController> logger,
                                  IMenuMgt menuMgr)
        {
            this.logger = logger;
            this._menuMgr = menuMgr;
        }

        [HttpGet(Name = "GetAlimentos")]
        public IActionResult ObtenerAlimentos()
        {
            return new JsonResult(_menuMgr.ObtenerAlimentos());
        }
    }
}
