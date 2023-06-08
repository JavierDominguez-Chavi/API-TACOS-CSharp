using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TACOS.Modelos;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class ResenasController
    {
        private readonly ILogger<MiembroController> logger;
        private IConsultanteMgt _consultanteMgr;

        public ResenasController(ILogger<MiembroController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            _consultanteMgr = consultanteMgr;
        }


        /// <summary>
        /// ObtenerResenas(). Regresa una lista con todos las reseñas.
        /// </summary>
        /// <remarks>
        /// Las reseñas se recuperan solo con el Nombre y Apellido paterno de la persona.
        /// </remarks>
        /// <response code="200">
        /// La petición fue aceptada.
        /// </response>
        /// <response code="409">El servidor tiene conflictos para procesar la peticion.</response>
        /// <returns>Lista de Reseñas</returns>
        [ProducesResponseType(typeof(List<Resena>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
        [Produces("application/json")]

        [HttpGet(Name = "ObtenerResenas")]
        public IActionResult ObtenerResenas()
        {
            try
            {
                return new JsonResult(_consultanteMgr.ObtenerResenas()) { StatusCode = 202 };
            }
            catch (HttpRequestException httpRequestException)
            {
                string mensaje = "";
                switch (httpRequestException.Message)
                {
                    case "409":
                        mensaje = "El servidor tiene conflictos por favor reintenta";
                        break;
                    default:
                        mensaje = "No hay conexión con la base de datos.";
                        break;
                }
                return new JsonResult(new { mensaje })
                { StatusCode = Int32.Parse(httpRequestException.Message) };
            }
        }



        /// <summary>
        /// BorrarResena(int idResena). Borra una reseña seleccionada mediante su Id.
        /// </summary>
        /// <remarks>
        /// Soolo es necesario el Id de la reseña para el proceso.
        /// </remarks>
        /// <response code="204">
        /// La operación fue exitosa.
        /// </response>
        /// <response code="400">Se requiere un ID de reseña válido para eliminar el registro.</response>
        /// <response code="404">Ningun registro coincide con la reseña que desea eliminar.</response>
        /// <response code="500">Error de servidor, intente de nuevo más tarde.</response>
        /// <returns>Resultado sin contenido</returns>
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]

        [HttpPost(Name = "BorrarResena")]
        //[Authorize]
        public IActionResult BorrarResena(int idResena)
        {
            try
            {
                _consultanteMgr.BorrarResena(idResena);
                return new NoContentResult();
            }
            catch (HttpRequestException httpRequestException)
            {
                Dictionary<string, string> respuestasExcepcion = new Dictionary<string, string>();
                respuestasExcepcion.Add("400", "Se requiere un ID de reseña válido para eliminar el registro.");
                respuestasExcepcion.Add("404", "Ningun registro coincide con la reseña que desea eliminar.");
                respuestasExcepcion.Add("500", "Error de servidor, intente de nuevo más tarde.");

                return new JsonResult(new { mensaje = respuestasExcepcion[httpRequestException.Message] })
                { StatusCode = Int32.Parse(httpRequestException.Message) };
            }
        }
    }
}
