using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Controladores.menu;
using TACOS.Modelos;
using Microsoft.EntityFrameworkCore;

using SistemaDeEmail;


namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class MiembroController : ControllerBase
    {
        private readonly ILogger<MiembroController> logger;
        private IConsultanteMgt _consultanteMgr;
        public MiembroController(ILogger<MiembroController> logger,
                                 IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            this._consultanteMgr = consultanteMgr;
        }

        /// <summary>
        /// ConfirmarRegistro(). Completa registro de miembro.
        /// </summary>
        /// <remarks>
        /// Usa el código de confirmación para completar el registro de un miembro. 
        /// La petición sólo necesita el IdMiembro y el CodigoConfirmacion.
        /// </remarks>
        /// <response code="200">La petición fue aceptada.</response>
        /// <response code="401">El código es incorrecto.</response>
        /// <response code="404">No se encontró el miembro solicitado.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Miembro con el código de confirmación limpio.</returns>
        [ProducesResponseType(typeof(Miembro), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpPut(Name = "ConfirmarRegistro")]
        public IActionResult ConfirmarRegistro(Miembro miembro)
        {
            try
            {
                this._consultanteMgr.ConfirmarRegistro(miembro);
                return new JsonResult(miembro) { StatusCode = 200 };
            }
            catch (Exception ex) 
            {
                string mensaje = "";
                switch (ex.Message)
                {
                    case "404":
                        mensaje = "No se encontró el miembro solicitado.";
                        break;
                    case "401":
                        mensaje = "El código es incorrecto.";
                        break;
                    default:
                        mensaje = "No hay conexión con la base de datos.";
                        break;
                }
                return new JsonResult(new { mensaje }) { StatusCode = Int32.Parse(ex.Message) };
            }
        }


        [HttpPost(Name = "RegistrarMiembro")]
        public IActionResult RegistrarMiembro(Miembro miembro)
        {
            try
            {
                this._consultanteMgr.RegistrarMiembro(miembro);
                this.EnviarCodigoConfirmacion(miembro);
                return new JsonResult(miembro) { StatusCode = 200 };
            }
            catch (Exception ex)
            {
                string mensaje = "";
                switch (ex.Message)
                {
                    case "422":
                        mensaje = "Ya existe una cuenta con este email.";
                        break;
                    case "500":
                        mensaje = "No hay conexión con la base de datos.";
                        break;
                }
                return new JsonResult(new { mensaje }) { StatusCode = Int32.Parse(ex.Message) };
            }
            
        }

        private void EnviarCodigoConfirmacion(Miembro miembro)
        {
            new ServicioEmail().EnviarCodigoConfirmacion(
                miembro.Persona.Email,
                (int)miembro.CodigoConfirmacion!
            );
        }


    }
}
