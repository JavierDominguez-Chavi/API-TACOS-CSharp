using TACOS.Modelos;

namespace TACOS.Negocio.PeticionesRespuestas;

public class RespuestaCredenciales
{
    /*
    private Miembro miembro;
    public Miembro Miembro
    {
        set
        {
            this.Persona.Miembros.Add(value);
            this.miembro = value;
        }
        get { return this.miembro; }
    }
    */
    public Miembro Miembro { get; set; }
    public string Token { get; set; }
    public string Expera { get; set; }
}
