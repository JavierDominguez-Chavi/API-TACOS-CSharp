namespace TACOS.Negocio;
using System.Collections.Generic;
using System.Globalization;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.ObjectModel;
using System.Threading.Channels;

public class MenuMgr : ManagerBase, IMenuMgt
{
    public MenuMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }

    public int ActualizarExistencia(Alimentospedido alimento)
    {
        Alimento? alimentoEncontrado = 
            this.tacosdbContext.Alimentos.FirstOrDefault(a => a.Id == alimento.IdAlimento);
        if (alimentoEncontrado is null)
        {
            throw new Exception("404");
        }
        if (alimento.Cantidad < 0 && alimentoEncontrado.Existencia < 1)
        {
            throw new Exception("409");
        }

        alimentoEncontrado.Existencia += alimento.Cantidad;
        return this.tacosdbContext.SaveChanges();
    }

    public List<Alimento> ObtenerAlimentos()
    {
        return this.tacosdbContext.Alimentos.OrderBy(a => a.Nombre)
                                            .ToList();
    }

}