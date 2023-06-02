using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Controladores.menu;
using TACOS.Modelos;
using JWTTokens;
using TACOS.Negocio.PeticionesRespuestas;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private IConsultanteMgt _consultanteMgr;
        private JwtTokenHandler jwtTokenHandler;

        public LoginController(ILogger<LoginController> logger,
                                  IConsultanteMgt consultanteMgr,
                                  JwtTokenHandler jwtTokenHandler)
        {
            this.logger = logger;
            this._consultanteMgr = consultanteMgr;
            this.jwtTokenHandler=jwtTokenHandler;
        }

        /// <summary>
        /// IniciarSesion().
        /// </summary>
        /// <remarks>
        /// Autentica las credenciales (email y contraseña) de un Miembro para permitirle entrar.
        /// Regresa la información del miembro si la encuentra, incluyendo su información de empleado, 
        /// en caso de encontrar alguna.
        /// </remarks>
        /// <response code="200">La petición fue aceptada.</response>
        /// <response code="400">Campos vacíos.</response>
        /// <response code="401">No se encontró ninguna cuenta con ese email y/o contraseña.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Miembro con el código de confirmación limpio.</returns>
        [ProducesResponseType(typeof(RespuestaCredenciales), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
        [Produces("application/json")]
        [HttpPost(Name = "IniciarSesion")]
        public IActionResult IniciarSesion([FromBody] Credenciales credenciales)
        {
            try
            {
                Miembro miembro = this._consultanteMgr.IniciarSesion(credenciales);
                var token = jwtTokenHandler.GenerarToken(
                    miembro.Persona.Email,
                    $"{miembro.Persona.Nombre} {miembro.Persona.ApellidoPaterno} {miembro.Persona.ApellidoMaterno}",
                    miembro.Id.ToString()
                );
                return new JsonResult(new
                {
                    miembro = miembro,
                    staff = "Aqui iria el staff",
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
