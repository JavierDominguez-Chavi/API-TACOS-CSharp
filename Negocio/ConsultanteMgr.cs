namespace TACOS.Negocio;
using System.Globalization;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Linq;

public class ConsultanteMgr : ManagerBase, IConsultanteMgt
{
    public ConsultanteMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }

    public Persona IniciarSesion(Persona credenciales)
    {
        if (credenciales is null
            || String.IsNullOrWhiteSpace(credenciales.Email)
            || String.IsNullOrWhiteSpace(credenciales.Miembros.ElementAt(0).Contrasena))
        {
            throw new ArgumentException("400");
        }

        Persona? personaEncontrada =
            this.tacosdbContext.Personas.FirstOrDefault(p => p.Email!.Equals(credenciales.Email));
        if (personaEncontrada is null)
        {
            throw new ArgumentException("401");
        }

        Miembro? miembroDePersonaEncontrada =
            this.tacosdbContext.Miembros.FirstOrDefault(m =>
                m.IdPersona == personaEncontrada.Id
                && m.Contrasena!.Equals(credenciales.Miembros.ElementAt(0).Contrasena)
            );
        if (miembroDePersonaEncontrada is null)
        {
            throw new ArgumentException("401");
        }

        personaEncontrada.Miembros.Add(miembroDePersonaEncontrada);
        return personaEncontrada;
    }

    public bool RegistrarMiembro(Persona persona)
    {
        bool confirmacion = false;
        this.tacosdbContext.Personas.Add(persona);
        try
        {
            int columnasAfectadas = this.tacosdbContext.SaveChanges();
            confirmacion =
                (AsignarCodigoConfirmacion(persona)
                && columnasAfectadas > 0);
        }
        catch (DbUpdateException)
        {
            throw new ArgumentException("422");
        }
        if (!confirmacion)
        {
            throw new ArgumentException("500");
        }
        return confirmacion;
    }

    public bool AsignarCodigoConfirmacion(Persona persona)
    {
        Miembro miembroEncontrado =
            this.tacosdbContext.Miembros.FirstOrDefault(m => m.IdPersona == persona.Id);
        if (miembroEncontrado != null)
        {
            miembroEncontrado!.CodigoConfirmacion = new Random().Next(10000, 100000);
            persona.Miembros.ElementAt(0).CodigoConfirmacion =
                miembroEncontrado.CodigoConfirmacion;
        }
        return this.tacosdbContext.SaveChanges() > 0;
    }

    public bool ConfirmarRegistro(Persona persona)
    {
        Miembro miembro = persona.Miembros.ElementAt(0);
        Miembro miembroEncontrado =
            this.tacosdbContext.Miembros.SingleOrDefault(m => m.Id == miembro.Id);
        if (miembroEncontrado is null)
        {
            throw new ArgumentException("404");
        }
        if (miembroEncontrado.CodigoConfirmacion != miembro.CodigoConfirmacion)
        {
            throw new ArgumentException("401");
        }
        miembroEncontrado.CodigoConfirmacion = 0;
        miembro.CodigoConfirmacion = 0;
        bool confirmacion = this.tacosdbContext.SaveChanges() > 0;
        if (!confirmacion)
        {
            throw new ArgumentException("500");
        }
        return confirmacion;
    }

    public List<Pedido> ObtenerPedidos()
    {
        List<Pedido> pedidos = this.tacosdbContext.Pedidos.OrderBy(p => p.Fecha).ToList();
        int numPedidos = pedidos.Count();
        for (int i = 0; i < numPedidos; i++)
        {
            Miembro? miembroPedido = this.tacosdbContext
                                        .Miembros
                                        .FirstOrDefault(m => m.Id == pedidos[i].IdMiembro);
            if (miembroPedido != null)
            {
                miembroPedido.Persona = this.tacosdbContext
                                            .Personas
                                            .FirstOrDefault(p => p.Id == miembroPedido.IdPersona);
            }
            pedidos[i].Miembro = miembroPedido;
        }
        return pedidos;
    }

    public bool RegistrarPedido(Pedido nuevoPedido)
    {
        this.tacosdbContext.Pedidos.Add(nuevoPedido);
        return this.tacosdbContext.SaveChanges() > 0;
    }

    public bool ActualizarPedido(PedidoSimple pedidoActualizado)
    {
        if (pedidoActualizado is null
            || pedidoActualizado.Id is 0
            || pedidoActualizado.IdMiembro is 0)
        {
            throw new HttpRequestException("400");
        }

        Pedido pedidoEncontrado =
            this.tacosdbContext.Pedidos.FirstOrDefault(p => p.Id == pedidoActualizado!.Id);
        if (pedidoEncontrado is null)
        {
            throw new HttpRequestException("404");
        }

        if (pedidoActualizado.IdMiembro != pedidoEncontrado.IdMiembro)
        {
            throw new HttpRequestException("422");
        }

        if (pedidoEncontrado.Estado == 3)
        {
            throw new HttpRequestException("403");
        }

        pedidoEncontrado.Estado = pedidoActualizado.Estado;
        ActualizarPedidosPagados(pedidoEncontrado, pedidoEncontrado);

        bool confirmacion = this.tacosdbContext.SaveChanges() > 0;
        if (!confirmacion)
        {
            throw new HttpRequestException("500");
        }
        return confirmacion;
    }
    public void ActualizarPedidosPagados(
        Pedido pedidoActualizado,
        Pedido pedidoEncontrado)
    {
        if (pedidoActualizado.Estado == 3)
        {
            Miembro miembroDelPedido =
                this.tacosdbContext.Miembros.FirstOrDefault(m => m.Id == pedidoActualizado.IdMiembro);
            if (miembroDelPedido is null)
            {
                throw new HttpRequestException("404");
            }

            miembroDelPedido.PedidosPagados += 1;
        }
    }

    public List<Resena> ObtenerResenas()
    {
        List<Resena> resenas = this.tacosdbContext.Resenas.OrderBy(a => a.Fecha).ToList();
        if (resenas is null)
        {
            throw new HttpRequestException("409");
        }
        List<Resena> resenasObtenidas = new List<Resena>();
        foreach (Resena resena in resenas)
        {
            var idPersona = (from Miembro in tacosdbContext.Miembros
                                       where Miembro.Id == resena.IdMiembro
                                       select Miembro.IdPersona).FirstOrDefault();
            var miembro = resena.Miembro;
            miembro.IdPersona = idPersona;
            resena.Miembro = miembro;
            var persona =
                   (from Persona in tacosdbContext.Personas
                    join Miembro in tacosdbContext.Miembros
                    on Persona.Id equals Miembro.IdPersona
                    where Miembro.Id == resena.IdMiembro
                    select Persona).ToList();
            miembro.Persona = new Persona();
            miembro.Persona.Nombre = persona.FirstOrDefault().Nombre;
            miembro.Persona.ApellidoPaterno = persona.FirstOrDefault().ApellidoPaterno;
            resenasObtenidas.Add(resena);
        }
        return resenasObtenidas;
    }
}
