namespace TACOS.Controladores.menu
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using TACOS.Modelos;
    using TACOS.Negocio.Interfaces;
    using TACOS.Negocio;
    using System.Net;
    using System.ComponentModel;

    [ApiController]
    [Route("menu/[controller]")]
    [Produces("application/json")]
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


    }
}
