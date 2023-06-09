using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;

namespace TACOS.Controladores.personas
{
    /// <summary>
    /// Controlador para el manejo de reseñas.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ResenasController
    {
        private readonly ILogger<MiembroController> logger;
        private IConsultanteMgt _consultanteMgr;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        /// <param name="logger">Inyectado por dependencias.</param>
        /// <param name="consultanteMgr">Componente para operaciones con personas. Inyectado por dependencias.</param>
        public ResenasController(ILogger<MiembroController> logger,
                                  IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            this._consultanteMgr = consultanteMgr;
        }

        [HttpGet(Name = "ObtenerResenas")]
        public IActionResult ObtenerResenas()
        {
            return new JsonResult(this._consultanteMgr.ObtenerResenas()) 
            { 
                StatusCode = 202 
            };
        }
    }
}
