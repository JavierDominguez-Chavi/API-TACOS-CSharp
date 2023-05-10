namespace TACOS.Negocio;
using System.Collections.Generic;
using System.Globalization;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;

public class MenuMgr : ManagerBase, IMenuMgt
{
    public MenuMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
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