namespace TACOSMenuAPI.Negocio;
using System.Collections.Generic;
using System.Globalization;
using TACOSMenuAPI.Negocio.Interfaces;
using TACOSMenuAPI.Modelos;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.ObjectModel;
using System.Threading.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

public class MenuMgr : ManagerBase, IMenuMgt
{
    public MenuMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }

    public Dictionary<int,int> ActualizarExistenciaAlimentos
        ([FromBody] Dictionary<int,int> idAlimentos_Cantidades)
    {
        int cantidadAlimentos = idAlimentos_Cantidades.Count;
        Alimento[] alimentos = new Alimento[cantidadAlimentos];
        for (int i = 0; i < cantidadAlimentos; i++)
        {
            KeyValuePair<int,int> registro = idAlimentos_Cantidades.ElementAt(i);
            Alimento? alimentoEncontrado = this.tacosdbContext.Alimentos
                                                              .FirstOrDefault(a => a.Id == registro.Key);
            if (alimentoEncontrado is null)
            {
                throw new ArgumentNullException(
                    $"No hay un alimento con el id {registro.Key}"
                );
            }
            if (registro.Value < 0 && alimentoEncontrado.Existencia < 1)
            {
                throw new HttpRequestException("409");
            }
            alimentos[i] = alimentoEncontrado;
        }
        foreach (Alimento alimento in alimentos)
        {
            alimento.Existencia += idAlimentos_Cantidades[alimento.Id];
        }
        if (this.tacosdbContext.SaveChanges() != cantidadAlimentos)
        {
            throw new HttpRequestException("500");
        }

        Dictionary<int,int> nuevasExistencias = new Dictionary<int, int>();
        foreach (Alimento alimento in alimentos)
        {
            nuevasExistencias.Add(alimento.Id, (int)alimento.Existencia);
        }
        return nuevasExistencias;
    }

    public List<Alimento> ObtenerAlimentosSinImagenes()
    {
        return this.tacosdbContext.Alimentos.OrderBy(a => a.Nombre)
                                            .ToList();
    }

}