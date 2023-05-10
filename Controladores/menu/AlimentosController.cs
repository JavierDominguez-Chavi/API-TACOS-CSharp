namespace TACOS.Controladores.menu
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using TACOS.Modelos;
    using TACOS.Negocio.Interfaces;
    using TACOS.Negocio;


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
            return new JsonResult(_menuMgr.ObtenerAlimentos()) { StatusCode = 202 };
        }

        [HttpPatch("{idAlimento}, {cantidad}",Name = "ActualizarExistencia")]
        public IActionResult ActualizarExistencia(int idAlimento, int cantidad)
        {
            try
            {
                return new JsonResult(this._menuMgr.ActualizarExistencia(
                    new Alimentospedido() { IdAlimento = idAlimento, Cantidad = cantidad}
                ));
            }
            catch (KeyNotFoundException knfException)
            {
                return new JsonResult(new Error { Mensaje = knfException.Message }) { StatusCode = 404 };
            }
            catch (ArgumentException aException)
            {
                return new JsonResult(new Error { Mensaje = aException.Message }) { StatusCode = 409 };
            }
        }


    }
}
