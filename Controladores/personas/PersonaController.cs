using Microsoft.AspNetCore.Mvc;
using TACOS.Negocio;
using TACOS.Negocio.Interfaces;
using TACOS.Controladores.menu;
using TACOS.Modelos;
using Microsoft.EntityFrameworkCore;

using SistemaDeEmail;
using TACOS.Modelos.PeticionesRespuestas;

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
