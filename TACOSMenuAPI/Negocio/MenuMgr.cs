#pragma warning disable CS1591
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
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Implementación de la interfaz IMenuMgt.
/// </summary>
public class MenuMgr : ManagerBase, IMenuMgt
{
    /// <summary></summary>
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
            //-1 = agotado.
            if (alimento.Existencia == 0)
            {
                alimento.Existencia = -1;
            }
            if (alimento.Existencia > -1)
            {
                alimento.Existencia += idAlimentos_Cantidades[alimento.Id];
            }
        }
        if (this.tacosdbContext.SaveChanges() != cantidadAlimentos)
        {
            return new Respuesta<Dictionary<int, int>>
            { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }

        Dictionary<int,int> nuevasExistencias = new Dictionary<int, int>();
        foreach (Alimento alimento in alimentos)
        {
            nuevasExistencias.Add(alimento.Id, (int)alimento.Existencia!);
        }
        return new Respuesta<Dictionary<int, int>>
            { Codigo = 200, Mensaje = Mensajes.Exito, Datos = nuevasExistencias };
    }

    public Respuesta<List<AlimentoActualizar>> ActualizarAlimentos(List<AlimentoActualizar> alimentos)
    {
        List<AlimentoActualizar> alimentosModificados = new List<AlimentoActualizar>();
        Respuesta<List<AlimentoActualizar>> respuesta = new Respuesta<List<AlimentoActualizar>>();
        foreach (AlimentoActualizar alimento in alimentos)
        {
            Alimento? alimentoEncontrado = this.tacosdbContext.Alimentos.Find(alimento.Id);
            
            if (alimentoEncontrado is null) 
            {
                respuesta.Mensaje = Mensajes.ActualizarAlimento_Parcial;
                continue; 
            }
            
            alimentoEncontrado.Nombre = alimento.Nombre;
            alimentoEncontrado.Descripcion = alimento.Descripcion;
            alimentoEncontrado.Existencia = alimento.Existencia;
            alimentoEncontrado.Precio= alimento.Precio;
            alimentosModificados.Add(new AlimentoActualizar(alimentoEncontrado));
        }
        try
        {
            this.tacosdbContext.SaveChanges();
            respuesta.Codigo = 200;
        }
        catch (DbUpdateException)
        {
            respuesta.Mensaje = Mensajes.ErrorInterno;
            respuesta.Codigo = 500;
        }
        if (String.IsNullOrEmpty(respuesta.Mensaje))
        {
            respuesta.Mensaje = Mensajes.Exito;
        }

        respuesta.Datos = alimentosModificados;
        
        return respuesta;
    }

    public Respuesta<Alimento[]> ObtenerAlimentosSinImagenes()
    {
        return new Respuesta<Alimento[]>
        {
            Codigo = 200,
            Mensaje = Mensajes.Exito,
            Datos = this.tacosdbContext.Alimentos.OrderBy(a => a.Nombre).ToArray()
        };
    }

    public Respuesta<Alimento[]> ObtenerAlimentosConImagenes()
    {
        return new Respuesta<Alimento[]>
        {
            Codigo = 200,
            Mensaje = Mensajes.Exito,
            Datos = this.tacosdbContext.Alimentos.Include(a => a.Imagen).ToArray()
        };
    }

    public Respuesta<Alimento> RegistrarAlimento(Alimento alimento)
    {
        this.tacosdbContext.Alimentos.Add(alimento);
        this.tacosdbContext.SaveChanges();
        return new Respuesta<Alimento> { Codigo = 200, Mensaje= Mensajes.Exito, Datos=alimento };
    }
}