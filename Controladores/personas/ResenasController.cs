using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class ResenasController
    {
        private readonly ILogger<MiembroController> logger;
        private IConsultanteMgt _consultanteMgr;

        public ResenasController(ILogger<MiembroController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            _consultanteMgr = consultanteMgr;
        }

        [HttpGet(Name = "ObtenerResenas")]
        public IActionResult ObtenerResenas()
        {
            return new JsonResult(_consultanteMgr.ObtenerResenas()) 
            { 
                StatusCode = 202 
            };
        }
    }
}
