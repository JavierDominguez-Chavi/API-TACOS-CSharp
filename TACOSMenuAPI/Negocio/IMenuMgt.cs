using System.Collections.ObjectModel;
using TACOSMenuAPI.Modelos;

namespace TACOSMenuAPI.Negocio.Interfaces
{
    public interface IMenuMgt
    {
        public List<Alimento> ObtenerAlimentosSinImagenes();
        public Dictionary<int,int> ActualizarExistenciaAlimentos(Dictionary <int, int> idAlimentos_Cantidades);
    }
}
