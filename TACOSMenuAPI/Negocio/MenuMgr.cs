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

    public Respuesta<Dictionary<int,int>> ActualizarExistenciaAlimentos
        ([FromBody] Dictionary<int,int> idAlimentos_Cantidades)
    {
        int cantidadAlimentos = idAlimentos_Cantidades.Count;
        Alimento[] alimentos = new Alimento[cantidadAlimentos];
        for (int i = 0; i < cantidadAlimentos; i++)
        {
            KeyValuePair<int,int> registro = idAlimentos_Cantidades.ElementAt(i);
            Alimento? alimentoEncontrado = 
                this.tacosdbContext.Alimentos.FirstOrDefault(a => a.Id == registro.Key);
            if (alimentoEncontrado is null)
            {
                return new Respuesta<Dictionary<int,int>>
                    { Codigo = 404,Mensaje = Mensajes.ActualizarExistenciaAlimentos_404};
            }
            if ((alimentoEncontrado.Existencia + registro.Value) < 0)
            {
                return new Respuesta<Dictionary<int, int>>
                    { Codigo = 409, Mensaje = Mensajes.ActualizarExistenciaAlimentos_409 };
            }
            alimentos[i] = alimentoEncontrado;
        }
        foreach (Alimento alimento in alimentos)
        {
            alimento.Existencia += idAlimentos_Cantidades[alimento.Id];
        }
        if (this.tacosdbContext.SaveChanges() != cantidadAlimentos)
        {
            return new Respuesta<Dictionary<int, int>>
            { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }

        Dictionary<int,int> nuevasExistencias = new Dictionary<int, int>();
        foreach (Alimento alimento in alimentos)
        {
            nuevasExistencias.Add(alimento.Id, (int)alimento.Existencia);
        }
        return new Respuesta<Dictionary<int, int>>
            { Codigo = 200, Mensaje = Mensajes.Exito, Datos = nuevasExistencias };
    }

    public Respuesta<List<Alimento>> ObtenerAlimentosSinImagenes()
    {
        return new Respuesta<List<Alimento>>
        { 
            Codigo = 200, 
            Mensaje = Mensajes.Exito, 
            Datos = this.tacosdbContext.Alimentos.OrderBy(a => a.Nombre).ToList() 
        };
    }

}