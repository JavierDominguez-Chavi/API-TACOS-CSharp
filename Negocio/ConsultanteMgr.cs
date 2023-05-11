namespace TACOS.Negocio;
using System.Globalization;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;

public class ConsultanteMgr : ManagerBase, IConsultanteMgt
{
    public ConsultanteMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }

    public Persona IniciarSesion(Persona credenciales)
    {
        if (credenciales is null || 
            String.IsNullOrWhiteSpace(credenciales.Email))
        {
            throw new ArgumentException("El email no puede estar vacío.");
        }
        if (String.IsNullOrWhiteSpace(credenciales.Miembros.ElementAt(0).Contrasena)) 
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
                && m.Contrasena!.Equals(credenciales.Miembros.ElementAt(0).Contrasena)
            );
        if (miembroDePersonaEncontrada is null)
        {
            throw new ArgumentException("No se encontró ninguna cuenta con ese email y/o contraseña.");
        }
        personaEncontrada.Miembros.Add(miembroDePersonaEncontrada);

        return personaEncontrada;
    }

    public bool RegistrarMiembro(Persona persona)
    {
        this.tacosdbContext.Personas.Add(persona);
        return this.tacosdbContext.SaveChanges() >0;
    }
}
