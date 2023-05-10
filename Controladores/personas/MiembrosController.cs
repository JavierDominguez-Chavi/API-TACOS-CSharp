using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Controladores.menu;
using TACOS.Modelos;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("personas/[controller]")]
    public class MiembrosController : ControllerBase
    {
        private readonly ILogger<MiembrosController> logger;
        private IConsultanteMgt _consultanteMgr;
        public MiembrosController(ILogger<MiembrosController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            this._consultanteMgr = consultanteMgr;
        }

        [HttpPost(Name = "PostMiembros")]
        public IActionResult IniciarSesion([FromBody] Credenciales credenciales)
        {            
            try
            {
                return new JsonResult(this._consultanteMgr.IniciarSesion(credenciales));
            }
            catch (ArgumentException aException)
            {
                return new JsonResult(new Error { Mensaje = aException.Message }) { StatusCode = 401 };
            }
        }
    }
}
