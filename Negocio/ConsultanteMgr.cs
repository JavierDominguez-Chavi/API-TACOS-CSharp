namespace TACOS.Negocio;
using System.Globalization;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;

public class ConsultanteMgr : ManagerBase, IConsultanteMgt
{
    public ConsultanteMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }

    public Miembro IniciarSesion(Credenciales credenciales)
    {
        if (String.IsNullOrWhiteSpace(credenciales.Email))
        {
            throw new ArgumentException("El email no puede estar vacío.");
        }
        if (String.IsNullOrWhiteSpace(credenciales.Contrasena)) 
        {
            throw new ArgumentException("La contrasena no puede estar vacía.");
        };

        Persona? personaEncontrada = 
            this.tacosdbContext.Personas.FirstOrDefault(p => p.Email!.Equals(credenciales.Email));
        if (personaEncontrada is null)
        {
            throw new ArgumentException("No se encontró ninguna cuenta con ese email y/o contraseña.");
        }

        Miembro? miembroDePersonaEncontrada =
            this.tacosdbContext.Miembros.FirstOrDefault(m =>
                m.IdPersona == personaEncontrada.Id
                && m.Contrasena!.Equals(credenciales.Contrasena)
            );
        if (miembroDePersonaEncontrada is null)
        {
            throw new ArgumentException("No se encontró ninguna cuenta con ese email y/o contraseña.");
        }
        miembroDePersonaEncontrada.Persona = personaEncontrada;

        return miembroDePersonaEncontrada;
    }
}
