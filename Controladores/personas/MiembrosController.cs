using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Controladores.menu;
using TACOS.Modelos;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost(Name = "IniciarSesion")]
        public IActionResult IniciarSesion([FromBody] Persona credenciales)
        {
            try
            {
                return new JsonResult(this._consultanteMgr.IniciarSesion(credenciales))
                {
                    StatusCode = 200
                };
            }
            catch (ArgumentException exception)
            {
                string mensaje;
                switch (exception.Message)
                {
                    case "400":
                        mensaje = "Todos los campos son obligatorios.";
                        break;
                    case "401":
                        mensaje = "No se encontró ninguna cuenta con ese email y/o contraseña.";
                        break;
                    default:
                        mensaje = "No hay conexión con la base de datos.";
                        break;
                }
                return new JsonResult(new { mensaje })
                {
                    StatusCode = Int32.Parse(exception.Message)
                };
            }
        }
    }
}
