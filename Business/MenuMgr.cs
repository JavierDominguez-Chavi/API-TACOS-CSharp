namespace TACOS.Business;
using System.Collections.Generic;
using System.Globalization;

using TACOS.Models;

public class MenuMgr : IMenuMgt
{

    private TacosdbContext tacosdbContext;

    public MenuMgr(TacosdbContext tacosdbContext)
    {
        string culture = "es-MX";
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);
        this.tacosdbContext = tacosdbContext;
    }
    public List<Alimento> ObtenerAlimentos()
    {
        return this.tacosdbContext.Alimentos.OrderBy(a => a.Nombre)
                                            .ToList();
    }

    public void RegistrarPedido()
    {
        Console.WriteLine("registrar pedido!!!!");
    }
}