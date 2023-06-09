using System.Globalization;
using TACOS.Modelos;

namespace TACOS.Negocio
{
    /// <summary>
    /// Superclase de las Interfaces Manager.
    /// </summary>
    public class ManagerBase
    {
        /// <summary>
        /// Conexión a la base de datos, obtenida por inyección de dependencias.
        /// </summary>
        protected TacosdbContext tacosdbContext;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="tacosdbContext">Conexión inyectada a la base de datos.</param>
        public ManagerBase(TacosdbContext tacosdbContext)
        {
            string culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            this.tacosdbContext = tacosdbContext;
        }
    }
}
