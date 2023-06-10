using Microsoft.AspNetCore.Mvc;
using TACOS.Modelos;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;



namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class TurnosController
    {
        private readonly ILogger<StaffController> logger;
        private IConsultanteMgt _consultanteMgr;

        public TurnosController(ILogger<StaffController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            _consultanteMgr = consultanteMgr;
        }

        /// <summary>
        /// ObtenerTurnos(). Obtiene todos los turnos existentes en el sistema.
        /// </summary>
        /// <remarks>
        /// Se recupera la información completa de los turnos.
        /// </remarks>
        /// <response code="200">
        /// La operación fue exitosa.
        /// </response>
        /// <response code="500">Error de servidor, intente de nuevo más tarde.</response>
        /// <returns>Lista de Turnos</returns>
        [ProducesResponseType(typeof(List<Turno>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]


        [HttpGet(Name = "ObtenerTurnos")]
        //[Authorize]
        public IActionResult ObtenerTurnos()
        {
            try
            {
                return new JsonResult(_consultanteMgr.ObtenerTurnos()) { StatusCode = 202 };
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
