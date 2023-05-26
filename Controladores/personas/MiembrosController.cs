using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Controladores.menu;
using TACOS.Modelos;
using JWTTokens;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class MiembrosController : ControllerBase
    {
        private readonly ILogger<MiembrosController> logger;
        private IConsultanteMgt _consultanteMgr;
        private JwtTokenHandler jwtTokenHandler;

        public MiembrosController(ILogger<MiembrosController> logger,
                                  IConsultanteMgt consultanteMgr,
                                  JwtTokenHandler jwtTokenHandler)
        {
            this.logger = logger;
            this._consultanteMgr = consultanteMgr;
            this.jwtTokenHandler=jwtTokenHandler;
        }

        [HttpPost(Name = "IniciarSesion")]
        public IActionResult IniciarSesion([FromBody] Credenciales credenciales)
        {
            try
            {
                Persona persona = this._consultanteMgr.IniciarSesion(credenciales);
                var token = jwtTokenHandler.GenerarToken(
                    persona.Email,
                    $"{persona.Nombre} {persona.ApellidoPaterno} {persona.ApellidoMaterno}",
                    persona.Id.ToString()
                );
                return new JsonResult(new
                {
                    persona = persona,
                    token = token.Item1,
                    expira = token.Item2
                })
                { StatusCode = 200 };
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
