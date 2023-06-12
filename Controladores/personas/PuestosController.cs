#pragma warning disable CS1591
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TACOS.Modelos;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;


namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class PuestosController
    {
        private readonly ILogger<StaffController> logger;
        private IConsultanteMgt _consultanteMgr;

        public PuestosController(ILogger<StaffController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            _consultanteMgr = consultanteMgr;
        }

        /// <summary>
        /// ObtenerPuestos(). Obtiene todos los puestos existentes en el sistema.
        /// </summary>
        /// <remarks>
        /// Se recupera la información completa de los puestos.
        /// </remarks>
        /// <response code="200">
        /// La operación fue exitosa.
        /// </response>
        /// <response code="500">Error de servidor, intente de nuevo más tarde.</response>
        /// <returns>Lista de Puestos</returns>
        [ProducesResponseType(typeof(List<Puesto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]


        [HttpGet(Name = "ObtenerPuestos")]
        //[Authorize]
        public IActionResult ObtenerPuestos()
        {
            try
            {
                return new JsonResult(_consultanteMgr.ObtenerPuestos()) { StatusCode = 202 };
            }
            catch (HttpRequestException httpRequestException)
            {
                Dictionary<string, string> respuestasExcepcion = new Dictionary<string, string>();
                respuestasExcepcion.Add("500", "Error de servidor, intente de nuevo más tarde.");

                return new JsonResult(new { mensaje = respuestasExcepcion[httpRequestException.Message] })
                { StatusCode = Int32.Parse(httpRequestException.Message) };
            }
        }
    }
}
