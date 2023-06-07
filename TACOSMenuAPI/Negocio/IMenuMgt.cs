using System.Collections.ObjectModel;
using TACOSMenuAPI.Modelos;

namespace TACOSMenuAPI.Negocio.Interfaces
{
    public interface IMenuMgt
    {
        public Respuesta<List<Alimento>> ObtenerAlimentosSinImagenes();
        public Respuesta<Dictionary<int,int>> ActualizarExistenciaAlimentos(Dictionary <int, int> idAlimentos_Cantidades);
    }
}
