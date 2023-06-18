using System.Collections.ObjectModel;
using TACOSMenuAPI.Modelos;

namespace TACOSMenuAPI.Negocio.Interfaces
{
    /// <summary>
    /// Interfaz responsable por las operaciones del negocio, concernientes con el menú.
    /// </summary>
    public interface IMenuMgt
    {
        /// <summary>
        /// Recupera los alimentos SIN sus imágenes; esa responsabilidad pertenece
        /// A la API gRPC; TACOSImagenesAPI.
        /// </summary>
        /// <returns>Lista de Alimentos.</returns>
        public Respuesta<Alimento[]> ObtenerAlimentosSinImagenes();

        /// <summary>
        /// Recupera los alimentos con sus respectivas imágenes.
        /// </summary>
        /// <returns>Lista de Alimentos.</returns>
        public Respuesta<Alimento[]> ObtenerAlimentosConImagenes();

        /// <summary>
        /// Modifica existencias por lote.
        /// </summary>
        /// <param name="idAlimentos_Cantidades">El primer int es la IdAlimento, y el segundo 
        /// es la cantidad a sumar o restar de la existencia de dicho alimento.</param>
        /// <returns>Relación entre IdAlimentos y sus nuevas existencias.</returns>
        public Respuesta<Dictionary<int,int>> ActualizarExistenciaAlimentos(Dictionary <int, int> idAlimentos_Cantidades);

        /// <summary>
        /// Actualiza la información de los alimentos.
        /// </summary>
        /// <param name="alimentos">Lista de alimentos a modificar.</param>
        /// <returns>Lista de alimentos con su información ya modificada.</returns>
        public Respuesta<List<AlimentoActualizar>> ActualizarAlimentos(List<AlimentoActualizar> alimentos);
        public Respuesta<Alimento> RegistrarAlimento(Alimento alimento);
    }
}
