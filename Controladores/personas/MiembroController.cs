using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using Microsoft.EntityFrameworkCore;

using SistemaDeEmail;
using TACOS.Negocio.PeticionesRespuestas;

namespace TACOS.Controladores.personas
{
    /// <summary>
    /// Controlador responsable de las operaciones con Miembros registrados.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MiembroController : ControllerBase
    {
        private readonly ILogger<MiembroController> logger;
        private IConsultanteMgt _consultanteMgr;

        /// <param name="logger">Normalmente inyectado</param>
        /// <param name="consultanteMgr">Normalmente inyectado</param>
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
            Respuesta<Miembro> respuesta = this._consultanteMgr.ConfirmarRegistro(miembro);
            return new JsonResult(respuesta) { StatusCode = respuesta.Codigo };
        }

        /// <summary>
        /// RegistrarMiembro(). Registra Miembro con su respectiva Persona.
        /// </summary>
        /// <remarks>
        /// Si el registro es exitoso, se asigna un CodigoConfirmacion al Miembro y 
        /// se envía al Email proporcionado.
        /// </remarks>
        /// <response code="200">Operación exitosa.</response>
        /// <response code="422">Ya existe una cuenta con este email.</response>
        /// <response code="500">Error en el servidor.</response>
        /// <returns>Miembro con el código de confirmación limpio.</returns>
        [ProducesResponseType(typeof(Respuesta<Miembro>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<Miembro>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Respuesta<Miembro>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpPost(Name = "RegistrarMiembro")]
        public IActionResult RegistrarMiembro(Miembro miembro)
        {
            Respuesta<Miembro> respuesta = this._consultanteMgr.RegistrarMiembro(miembro);
            if (respuesta.Codigo == 200)
            {
                this.EnviarCodigoConfirmacion(miembro);
            }
            return new JsonResult(respuesta) { StatusCode = respuesta.Codigo };
        }


        private void EnviarCodigoConfirmacion(Miembro miembro)
        {
            new ServicioEmail().EnviarCodigoConfirmacion(
                miembro.Persona!.Email!,
                (int)miembro.CodigoConfirmacion!
            );
        }
    }
}
