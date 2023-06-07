using System.ComponentModel;
using System.Runtime.CompilerServices;
using TACOS.Modelos;

namespace TACOS.Negocio.PeticionesRespuestas;

public class RespuestaCredenciales
{
    public Miembro Miembro { get; set; } = null;
    public int Codigo { get; set; } = 500;
    public string Mensaje { get; set; }
    public string Token { get; set; }
    public int Expira { get; set; }
}