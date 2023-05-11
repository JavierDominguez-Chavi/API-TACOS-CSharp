using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Controladores.menu;
using TACOS.Modelos;
using Microsoft.EntityFrameworkCore;

namespace TACOS.Controladores.personas
{
    [ApiController]
    [Route("[controller]")]
    public class PersonaController : ControllerBase
    {
        private readonly ILogger<PersonaController> logger;
        private IConsultanteMgt _consultanteMgr;
        public PersonaController(ILogger<PersonaController> logger,
                                 IConsultanteMgt consultanteMgr)
        {
            this.logger = logger;
            this._consultanteMgr = consultanteMgr;
        }

        [HttpPost(Name = "RegistrarMiembro")]
        public IActionResult RegistrarMiembro(Persona persona)
        {
            try
            {
                return new JsonResult(
                    new{
                        correcto = this._consultanteMgr.RegistrarMiembro(persona),
                        registro = persona
                    }
                );
            }
            catch (DbUpdateException dbuException)
            {
                return new JsonResult(
                    new Error() { Mensaje = "Ya existe una cuenta con este email." }
                ){ StatusCode  = 422};
            }

        }
    }
}
