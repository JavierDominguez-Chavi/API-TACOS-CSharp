#pragma warning disable CS1591
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TACOS.Modelos;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;



namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class StaffController
    {
        private readonly ILogger<StaffController> logger;
        private IConsultanteMgt consultanteMgr;

        public StaffController(ILogger<StaffController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            this.consultanteMgr = consultanteMgr;
        }

        /// <summary>
        /// RegistrarEmpleadoStaff(). Registra un empleado en el Staff junto con los datos de su Persona.
        /// </summary>
        /// <remarks>
        /// Para el registro es necesario la informacion de Staff como de Persona.
        /// </remarks>
        /// <response code="204">
        /// La operación fue exitosa.
        /// </response>
        /// <response code="400">Mensaje con el error de validación proveniente de MiembroValidador</response>
        /// <response code="422">El nombre y/o email que desea registrar ya existe en el sistema.</response>
        /// <response code="500">Error de servidor, intente de nuevo más tarde.</response>
        /// <returns>>Resultado sin contenido</returns>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]

        [HttpPost(Name = "RegistrarEmpleadoStaff")]
        public IActionResult RegistrarEmpleadoStaff(Staff staff)
        {
            try
            {
                consultanteMgr.RegistrarEmpleadoStaff(staff);
                return new NoContentResult();
            }
            catch (HttpRequestException httpRequestException)
            {
                Dictionary<string, string> respuestasExcepcion = new Dictionary<string, string>();
                respuestasExcepcion.Add("400", httpRequestException.Message);
                respuestasExcepcion.Add("422", Mensajes.RegistrarEmpleadoStaff_422);
                respuestasExcepcion.Add("500", Mensajes.ErrorInterno);

                return new JsonResult(new { mensaje = respuestasExcepcion[(httpRequestException.Data["CodigoError"] as string)!] })
                { StatusCode = Int32.Parse((httpRequestException.Data["CodigoError"] as string)!) };
            }
        }
    }
}
