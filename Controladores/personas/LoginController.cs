using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using JWTTokens;
using TACOS.Negocio.PeticionesRespuestas;

namespace TACOS.Controladores.personas
{
    /// <summary>
    /// Controlador del servicio de inicio de sesión.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> logger;
        private IConsultanteMgt _consultanteMgr;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        /// <param name="logger">Inyectado por dependencias.</param>
        /// <param name="consultanteMgr">Componente para operaciones con personas. Inyectado por dependencias.</param>
        public LoginController(ILogger<LoginController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            this._consultanteMgr = consultanteMgr;
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
        [ProducesResponseType(typeof(RespuestaCredenciales), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(RespuestaCredenciales), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(RespuestaCredenciales), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost(Name = "IniciarSesion")]
        public IActionResult IniciarSesion([FromBody] Credenciales credenciales)
        {
            RespuestaCredenciales respuesta = this._consultanteMgr.IniciarSesion(credenciales);
            Tuple<string,int> token = new Tuple<string, int>("",0);
            if (respuesta.Codigo == 200)
            {
                token = JwtTokenHandler.GenerarToken(
                    respuesta.Asociado!.Persona!.Email!,
                    $"{respuesta.Asociado.Persona.Nombre} {respuesta.Asociado.Persona.ApellidoPaterno} {respuesta.Asociado.Persona.ApellidoMaterno}",
                    respuesta.Asociado.Contrasena!
                );
            }
            respuesta.Token = token.Item1;
            respuesta.Expira = token.Item2;
            return new JsonResult(respuesta){ StatusCode = respuesta.Codigo };
        }
    }
}
