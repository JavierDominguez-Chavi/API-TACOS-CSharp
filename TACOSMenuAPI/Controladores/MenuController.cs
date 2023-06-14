namespace TACOSMenuAPI.Controladores.menu
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using TACOSMenuAPI.Modelos;
    using TACOSMenuAPI.Negocio.Interfaces;
    using System.Net;
    using System.ComponentModel;
    using TACOSMenuAPI.Negocio;

    /// <summary>
    /// Controlador responsable de ofrecer los servicios del menú.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> logger;
        private readonly IMenuMgt menuMgr;

        /// <param name="logger"></param>
        /// <param name="menuMgr"></param>
        public MenuController(ILogger<MenuController> logger,
                                  IMenuMgt menuMgr)
        {
            this.logger = logger;
            this.menuMgr = menuMgr;
        }


        /// <summary>
        /// ObtenerAlimentosSinImagenes(). Regresa una lista con todos los Alimentos.
        /// </summary>
        /// <remarks>
        /// Los alimentos vienen sin imágenes; estas deben obtenerse del TACOSImagenesAPI vía gRPC.
        /// </remarks>
        /// <response code="200">
        /// La petición fue aceptada.
        /// </response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Lista de Alimentos</returns>
        [ProducesResponseType(typeof(Respuesta<List<Alimento>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<List<Alimento>>), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet(Name = "GetAlimentos")]

        public IActionResult ObtenerAlimentosSinImagenes()
        {
            try
            {
                return new JsonResult(this.menuMgr.ObtenerAlimentosSinImagenes()) { StatusCode = 200 };
            }
            catch (Exception)
            {
                return new JsonResult(new Respuesta<object> { Codigo=500, Mensaje=Mensajes.ErrorInterno }) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// Obtiene todos los alimentos con sus respectivas imagenes.
        /// </summary>
        /// <response code="200">Se recuperaron los alimentos.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Lista de Alimentos</returns>
        [ProducesResponseType(typeof(Respuesta<List<Alimento>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<List<Alimento>>), StatusCodes.Status500InternalServerError)]
        [HttpGet("ObtenerAlimentosConImagenes")]
        public IActionResult ObtenerAlimentosConImagenes()
        {
            try
            {
                return new JsonResult(this.menuMgr.ObtenerAlimentosConImagenes()) { StatusCode = 200 };
            }
            catch (Exception)
            {
                return new JsonResult(new Respuesta<object> { Codigo=500, Mensaje=Mensajes.ErrorInterno }) { StatusCode = 500 };
            }
        }


        /// <summary>
        /// ActualizarExistenciaAlimentos(). Actualiza la existencia de uno o varios alimentos.
        /// </summary>
        /// <param name="idAlimentos_Cantidades">Diccionario con los IDs de los alimentos a modificar, y las cantidades a sumar o restar de cada alimento.</param>
        /// <response code="200">Las existencias han sido modificadas. Regresa un diccionario con las IDs de los alimentos afectados, y as nuevas existencias por alimento.</response>
        /// <response code="404">No se encontró el alimento solicitado.</response>
        /// <response code="409">La existencia del alimento solicitado ya no puede decrecer.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Lista de Alimentos</returns>
        [ProducesResponseType(typeof(Respuesta<Dictionary<int, int>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<Dictionary<int, int>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Respuesta<Dictionary<int, int>>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Respuesta<Dictionary<int, int>>), StatusCodes.Status500InternalServerError)]
        [HttpPatch(Name = "ActualizarExistencia")]
        public IActionResult ActualizarExistenciaAlimentos([FromBody] Dictionary<int,int> idAlimentos_Cantidades)
        {
            try
            {
                var respuesta = this.menuMgr.ActualizarExistenciaAlimentos(idAlimentos_Cantidades);
                return new JsonResult(respuesta) { StatusCode = respuesta.Codigo };
            }
            catch (Exception)
            {
                return new JsonResult(new Respuesta<object> { Codigo=500, Mensaje=Mensajes.ErrorInterno}) { StatusCode = 500 };
            }
        }

        /// <summary>
        /// ActualizarAlimentos(). Modifica la información de alimentos.
        /// </summary>
        /// <param name="alimentosAModificar">Lista de alimentos por actualizar.</param>
        /// <response code="200">Los alimentos fueron modificados. Si algún alimento no fue encontrado, 
        /// la petición no se rechaza pero se menciona el acontecimiento en la respuesta.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Lista de Alimentos</returns>
        [ProducesResponseType(typeof(Respuesta<List<AlimentoActualizar>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Respuesta<List<AlimentoActualizar>>), StatusCodes.Status500InternalServerError)]
        [HttpPut(Name = "ActualizarAlimento")]
        public IActionResult ActualizarAlimentos([FromBody] List<AlimentoActualizar> alimentosAModificar)
        {
            try
            {
                var respuesta = this.menuMgr.ActualizarAlimentos(alimentosAModificar);
                return new JsonResult(respuesta) { StatusCode = respuesta.Codigo };
            }
            catch (Exception)
            {
                return new JsonResult(new Respuesta<object> { Codigo=500, Mensaje=Mensajes.ErrorInterno }) { StatusCode = 500 };
            }
        }
    }
}
