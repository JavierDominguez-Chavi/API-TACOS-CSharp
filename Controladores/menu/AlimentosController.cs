namespace TACOS.Controladores.menu
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using TACOS.Modelos;
    using TACOS.Negocio.Interfaces;
    using TACOS.Negocio;
    using System.Net;
    using System.ComponentModel;

    [ApiController]
    [Route("menu/[controller]")]
    [Produces("application/json")]
    public class AlimentosController : ControllerBase
    {
        private readonly ILogger<AlimentosController> logger;
        private IMenuMgt _menuMgr;

        public AlimentosController(ILogger<AlimentosController> logger,
                                  IMenuMgt menuMgr)
        {
            this.logger = logger;
            this._menuMgr = menuMgr;
        }


        /// <summary>
        /// ObtenerAlimentos(). Regresa una lista con todos los Alimentos.
        /// </summary>
        /// <remarks>
        /// Los alimentos vienen sin imágenes; estas deben obtenerse del TACOSImagenesAPI vía gRPC.
        /// </remarks>
        /// <response code="200">
        /// La petición fue aceptada.
        /// </response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Lista de Alimentos</returns>

        [ProducesResponseType(typeof(List<Alimento>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        [HttpGet(Name = "GetAlimentos")]

        public IActionResult ObtenerAlimentos()
        {
            try
            {
                return new JsonResult(_menuMgr.ObtenerAlimentos()) { StatusCode = 200 };
            }
            catch (Exception e)
            {
                return new JsonResult(new Error { Mensaje = "No hay conexión con la base de datos" }) { StatusCode = 500 };
            }
        }

        /// <description>
        /// asdf
        /// </description>
        /// <summary>
        /// ActualizarExistencia(). Actualiza la existencia de un alimento.
        /// </summary>
        /// <param name="idAlimento">Identificador del alimento a modificar.</param>
        /// <param name="cantidad">Cantidad de existencia a modificar. Negativo para restar; positivo para sumar.</param>
        /// <response code="200">La existencia ha sido modificada.</response>
        /// <response code="404">No se encontró el alimento solicitado.</response>
        /// <response code="409">La existencia del alimento solicitado ya no puede decrecer.</response>
        /// <response code="500">El servidor falló inesperadamente.</response>
        /// <returns>Lista de Alimentos</returns>
        [ProducesResponseType(typeof(Alimentospedido), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
        [HttpPatch("{idAlimento}, {cantidad}",Name = "ActualizarExistencia")]
        public IActionResult ActualizarExistencia(int idAlimento, int cantidad)
        {
            try
            {
                return new JsonResult(this._menuMgr.ActualizarExistencia(
                    new Alimentospedido() { IdAlimento = idAlimento, Cantidad = cantidad}
                ));
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    case "404":
                        return new JsonResult(
                            new Error { 
                                Mensaje = "No se encontró el alimento solicitado." }
                            ) { StatusCode = 404 };
                    case "409":
                        return new JsonResult(
                            new Error{
                                Mensaje = "La existencia del alimento solicitado ya no puede decrecer."
                            }){ StatusCode = 409 };
                    default:
                        return new JsonResult(
                            new Error{
                                Mensaje = "No hay conexión con la base de datos."
                            }){ StatusCode = 500 };
                }
            }
        }


    }
}
