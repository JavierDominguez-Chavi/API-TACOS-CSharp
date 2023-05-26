using System.Globalization;
using TACOSMenuAPI.Modelos;

namespace TACOSMenuAPI.Negocio
{
    public class ManagerBase
    {
        protected TacosdbContext tacosdbContext;

        public ManagerBase(TacosdbContext tacosdbContext)
        {
            string culture = "es-MX";
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
            this.tacosdbContext = tacosdbContext;
        }
    }
}
