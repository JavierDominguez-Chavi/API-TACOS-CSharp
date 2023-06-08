namespace TACOS.Negocio;
using System.Globalization;
using TACOS.Negocio.Interfaces;
using TACOS.Modelos;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Linq;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using TACOS.Negocio.PeticionesRespuestas;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Resources;
using System.Globalization;

public class ConsultanteMgr : ManagerBase, IConsultanteMgt
{
    public ConsultanteMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }


    public RespuestaCredenciales IniciarSesion(Credenciales credenciales)
    {
        string contrasena = credenciales.Contrasena;
        if (credenciales is null
            || String.IsNullOrWhiteSpace(credenciales.Email)
            || String.IsNullOrWhiteSpace(contrasena))
        {
            return new RespuestaCredenciales { Codigo = 400,Mensaje = Mensajes.IniciarSesion_400};
        }

        Persona? personaEncontrada =
            this.tacosdbContext.Personas.FirstOrDefault(p => p.Email!.Equals(credenciales.Email));
        if (personaEncontrada is null)
        {
            return new RespuestaCredenciales { Codigo = 401, Mensaje = Mensajes.IniciarSesion_401 };
        }

        Miembro? miembroDePersonaEncontrada =
            this.tacosdbContext.Miembros.FirstOrDefault(m =>m.IdPersona == personaEncontrada.Id);
        bool contrasenaCorrecta = false;
        try
        {
            contrasenaCorrecta =
                BCrypt.Verify(contrasena, miembroDePersonaEncontrada.Contrasena);
        }
        catch (SaltParseException)
        {
            return new RespuestaCredenciales { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        if (miembroDePersonaEncontrada is null || !contrasenaCorrecta)
        {
            return new RespuestaCredenciales { Codigo = 401, Mensaje = Mensajes.IniciarSesion_401 };
        }

        miembroDePersonaEncontrada.Persona = personaEncontrada;
        return new RespuestaCredenciales 
            { Miembro = miembroDePersonaEncontrada, Codigo = 200, Mensaje = Mensajes.OperacionExitosa };
    }

    public Respuesta<Miembro> RegistrarMiembro(Miembro miembro)
    {
        bool confirmacion = false;
        miembro.Contrasena = 
            BCrypt.HashPassword(miembro.Contrasena);
        Persona persona = miembro.Persona;
        persona.Miembros.Add(miembro);
        this.tacosdbContext.Personas.Add(persona);
        try
        {
            int columnasAfectadas = this.tacosdbContext.SaveChanges();
            confirmacion =
                (AsignarCodigoConfirmacion(miembro.Persona)
                && columnasAfectadas > 0);
        }
        catch (DbUpdateException)
        {
            return new Respuesta<Miembro> { Codigo = 422, Mensaje = Mensajes.RegistrarMiembro_422 };
        }
        if (!confirmacion)
        {
            return new Respuesta<Miembro> { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        return new Respuesta<Miembro> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos = miembro };
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

    public Respuesta<Miembro> ConfirmarRegistro(Miembro miembro)
    {
        Miembro miembroEncontrado =
            this.tacosdbContext.Miembros.SingleOrDefault(m => m.Id == miembro.Id);
        Console.WriteLine(miembro.Id);
        if (miembroEncontrado is null)
        {
            return new Respuesta<Miembro> { Codigo = 404, Mensaje = Mensajes.ConfirmarRegistro_404 };
        }
        if (miembroEncontrado.CodigoConfirmacion != miembro.CodigoConfirmacion)
        {
            return new Respuesta<Miembro> { Codigo = 401, Mensaje = Mensajes.ConfirmarRegistro_401 };
        }
        miembroEncontrado.CodigoConfirmacion = 0;
        miembro.CodigoConfirmacion = 0;
        if (! (this.tacosdbContext.SaveChanges() > 0))
        {
            return new Respuesta<Miembro> { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        return new Respuesta<Miembro> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos = miembro};
    }

    public Respuesta<List<PedidoReporte>> ObtenerPedidosEntre(RangoFecha rango)
    {
        List<Pedido> pedidosEncontrados =
            this.tacosdbContext
                .Pedidos
                .Where(p => p.Fecha >= rango.Desde 
                            && p.Fecha <= rango.Hasta)
                .Include(pedido => pedido.Alimentospedidos)
                .ThenInclude(alimentoPedido => alimentoPedido.Alimento)
                .Include(pedido => pedido.Miembro)
                .ThenInclude(miembro => miembro.Persona)
                .OrderBy(pedido => pedido.Id)
                .ToList();
        if (pedidosEncontrados.IsNullOrEmpty())
        {
            return new Respuesta<List<PedidoReporte>> { Codigo = 404, Mensaje = Mensajes.ObtenerPedidosEntre_404 };
        }
        List<PedidoReporte> pedidosReporte = new List<PedidoReporte>();
        foreach (Pedido pedido in pedidosEncontrados)
        {
            pedidosReporte.Add(new PedidoReporte(pedido));
        }
        return new Respuesta<List<PedidoReporte>> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos=pedidosReporte};
    }

    public Respuesta<Pedido> RegistrarPedido(Pedido nuevoPedido)
    {
        if (nuevoPedido.Alimentospedidos.IsNullOrEmpty())
        {
            return new Respuesta<Pedido> { Codigo = 400, Mensaje = Mensajes.RegistrarPedido_400};
        }
        foreach (Alimentospedido alimento in nuevoPedido.Alimentospedidos)
        {
            if (alimento.Cantidad < 1)
            {
                return new Respuesta<Pedido> { Codigo = 400, Mensaje = Mensajes.RegistrarPedido_400 };
            }
        }
        this.tacosdbContext.Pedidos.Add(nuevoPedido);
        try
        {
            this.tacosdbContext.SaveChanges();
        }
        catch (DbUpdateException)
        {
            return new Respuesta<Pedido> { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        return new Respuesta<Pedido> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos=nuevoPedido };
    }

    public Respuesta<Pedido> ActualizarPedido(PedidoSimple pedidoActualizado)
    {
        if (pedidoActualizado is null
            || pedidoActualizado.Id is 0
            || pedidoActualizado.IdMiembro is 0)
        {
            return new Respuesta<Pedido> { Codigo = 400, Mensaje = Mensajes.ActualizarPedido_400 };
        }

        Pedido pedidoEncontrado =
            this.tacosdbContext.Pedidos.FirstOrDefault(p => p.Id == pedidoActualizado!.Id);
        if (pedidoEncontrado is null)
        {
            return new Respuesta<Pedido> { Codigo = 404, Mensaje = Mensajes.ActualizarPedido_404 };
        }

        if (pedidoActualizado.IdMiembro != pedidoEncontrado.IdMiembro)
        {
            return new Respuesta<Pedido> { Codigo = 422, Mensaje = Mensajes.ActualizarPedido_422 };
        }

        if (pedidoEncontrado.Estado == 3)
        {
            return new Respuesta<Pedido> { Codigo = 403, Mensaje = Mensajes.ActualizarPedido_403 };
        }

        pedidoEncontrado.Estado = pedidoActualizado.Estado;
        try
        {
            ActualizarPedidosPagados(pedidoEncontrado);
        }
        catch (HttpRequestException)
        {
            return new Respuesta<Pedido> { Codigo = 404, Mensaje = Mensajes.ActualizarPedido_404 };
        }

        try
        {
            this.tacosdbContext.SaveChanges();
        }
        catch(DbUpdateException)
        {
            return new Respuesta<Pedido> { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        return new Respuesta<Pedido> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa , Datos= pedidoEncontrado };
    }
    public void ActualizarPedidosPagados(Pedido pedidoActualizado)
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

    public bool BorrarResena(int idResena)
    {
        if (idResena == null || idResena == 0)
        {
            throw new HttpRequestException("400");
        }
        var resenaEncontrada = tacosdbContext.Resenas.FirstOrDefault(r => r.Id == idResena);
        if (resenaEncontrada is null)
        {
            throw new HttpRequestException("404");
        }

        tacosdbContext.Resenas.Remove(resenaEncontrada);
        bool operacionExitosa = this.tacosdbContext.SaveChanges() > 0;

        if (!operacionExitosa)
        {
            throw new HttpRequestException("500");
        }
        return operacionExitosa;

    }
}
