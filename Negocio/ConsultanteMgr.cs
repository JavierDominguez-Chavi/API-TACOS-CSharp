namespace TACOS.Negocio;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using TACOS.Modelos;
using TACOS.Negocio.Interfaces;
using TACOS.Negocio.PeticionesRespuestas;

/// <summary>
/// Implementación de la interfaz IConsultanteMgt.
/// </summary>
public class ConsultanteMgr : ManagerBase, IConsultanteMgt
{
    #pragma warning disable CS1591
    public ConsultanteMgr(TacosdbContext tacosdbContext) : base(tacosdbContext)
    {
    }

    #pragma warning disable CS1591
    public RespuestaCredenciales IniciarSesion(Credenciales credenciales)
    {
        if (credenciales is null
            || String.IsNullOrWhiteSpace(credenciales.Email)
            || String.IsNullOrWhiteSpace(credenciales.Contrasena))
        {
            return new RespuestaCredenciales { Codigo = 400, Mensaje = Mensajes.IniciarSesion_400 };
        }
        string contrasena = credenciales.Contrasena!;

        Persona? personaEncontrada =
            this.tacosdbContext.Personas.FirstOrDefault(p => p.Email!.Equals(credenciales.Email));
        if (personaEncontrada is null)
        {
            return new RespuestaCredenciales { Codigo = 401, Mensaje = Mensajes.IniciarSesion_401 };
        }

        Miembro? miembroDePersonaEncontrada =
            this.tacosdbContext.Miembros.FirstOrDefault(m => m.IdPersona == personaEncontrada.Id);
        bool contrasenaCorrecta = false;
        if (miembroDePersonaEncontrada is null)
        {
            return new RespuestaCredenciales { Codigo = 401, Mensaje = Mensajes.IniciarSesion_401 };
        }
        try
        {
            contrasenaCorrecta =
                BCrypt.Verify(contrasena, miembroDePersonaEncontrada!.Contrasena);
        }
        catch (SaltParseException)
        {
            return new RespuestaCredenciales { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        if (!contrasenaCorrecta)
        {
            return new RespuestaCredenciales { Codigo = 401, Mensaje = Mensajes.IniciarSesion_401 };
        }

        miembroDePersonaEncontrada.Persona = personaEncontrada;
        return new RespuestaCredenciales
        { Miembro = miembroDePersonaEncontrada, Codigo = 200, Mensaje = Mensajes.OperacionExitosa };
    }

    public Respuesta<Miembro> RegistrarMiembro(Miembro miembro)
    {
        var resultados = new MiembroValidador().Validate(miembro);
        if (!resultados.IsValid)
        {
            return new Respuesta<Miembro> { Codigo = 400, Mensaje = resultados.Errors.First().ErrorMessage };
        }

        bool confirmacion = false;
        miembro.Contrasena =
            BCrypt.HashPassword(miembro.Contrasena);
        Persona persona = miembro.Persona!;
        persona!.Miembros.Add(miembro);
        this.tacosdbContext.Personas.Add(persona);
        try
        {
            int columnasAfectadas = this.tacosdbContext.SaveChanges();
            confirmacion =
                this.AsignarCodigoConfirmacion(miembro.Persona!)
                && columnasAfectadas > 0;
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

    /// <summary>
    /// Si la Persona tiene un Miembro, se le asigna a éste un CodigoConfirmacion.
    /// </summary>
    /// <param name="persona"></param>
    /// <returns>Confirmación de que se guardaron los datos.</returns>
    public bool AsignarCodigoConfirmacion(Persona persona)
    {
        Miembro? miembroEncontrado =
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
        Miembro? miembroEncontrado =
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
        if (!(this.tacosdbContext.SaveChanges() > 0))
        {
            return new Respuesta<Miembro> { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        return new Respuesta<Miembro> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos = miembro };
    }

    public Respuesta<List<PedidoReporte>> ObtenerPedidosEntre(RangoFecha rango)
    {
        List<Pedido> pedidosEncontrados =
            this.tacosdbContext
                .Pedidos
                .Where(p => ((DateTime)p.Fecha!).Date >= rango.Desde.Date
                            && ((DateTime)p.Fecha).Date <= rango.Hasta.Date)
                .Include(pedido => pedido.Alimentospedidos)
                .ThenInclude(alimentoPedido => alimentoPedido.Alimento)
                .Include(pedido => pedido.Miembro)
                .ThenInclude(miembro => miembro!.Persona)
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
        return new Respuesta<List<PedidoReporte>> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos = pedidosReporte };
    }

    public Respuesta<Pedido> RegistrarPedido(Pedido nuevoPedido)
    {
        if (nuevoPedido.Alimentospedidos.IsNullOrEmpty())
        {
            return new Respuesta<Pedido> { Codigo = 400, Mensaje = Mensajes.RegistrarPedido_400 };
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
        return new Respuesta<Pedido> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos = nuevoPedido };
    }

    public Respuesta<Pedido> ActualizarPedido(PedidoSimple pedidoActualizado)
    {
        if (pedidoActualizado is null
            || pedidoActualizado.Id is 0
            || pedidoActualizado.IdMiembro is 0)
        {
            return new Respuesta<Pedido> { Codigo = 400, Mensaje = Mensajes.ActualizarPedido_400 };
        }

        Pedido? pedidoEncontrado =
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
            this.ActualizarPedidosPagados(pedidoEncontrado);
        }
        catch (HttpRequestException)
        {
            return new Respuesta<Pedido> { Codigo = 404, Mensaje = Mensajes.ActualizarPedido_404 };
        }

        try
        {
            this.tacosdbContext.SaveChanges();
        }
        catch (DbUpdateException)
        {
            return new Respuesta<Pedido> { Codigo = 500, Mensaje = Mensajes.ErrorInterno };
        }
        return new Respuesta<Pedido> { Codigo = 200, Mensaje = Mensajes.OperacionExitosa, Datos = pedidoEncontrado };
    }

    /// <summary>
    /// Actualiza los pedidos pagados del Miembro al cual le pertenece el Pedido actualizado.
    /// </summary>
    /// <param name="pedidoActualizado">Pedido cuyo Estado acaba de cambiar.</param>
    /// <exception cref="HttpRequestException">El Miembro no existe.</exception>
    public void ActualizarPedidosPagados(Pedido pedidoActualizado)
    {
        if (pedidoActualizado.Estado == 3)
        {
            Miembro? miembroDelPedido =
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
            throw new HttpRequestException("500");
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
                   (from Persona in this.tacosdbContext.Personas
                    join Miembro in this.tacosdbContext.Miembros
                    on Persona.Id equals Miembro.IdPersona
                    where Miembro.Id == resena.IdMiembro
                    select Persona).ToList();
            miembro.Persona = new Persona();
            miembro.Persona.Nombre = persona.FirstOrDefault()!.Nombre;
            miembro.Persona.ApellidoPaterno = persona.FirstOrDefault()!.ApellidoPaterno;
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

    public List<Puesto> ObtenerPuestos()
    {
        List<Puesto> puestos = this.tacosdbContext.Puestos.ToList();
        if (puestos is null)
        {
            throw new HttpRequestException("500");
        }
        return puestos;
    }

    public List<Turno> ObtenerTurnos()
    {
        List<Turno> turnos = this.tacosdbContext.Turnos.ToList();
        if (turnos is null)
        {
            throw new HttpRequestException("500");
        }
        return turnos;
    }

    public void RegistrarEmpleadoStaff(Staff staff)
    {
        var validacionStaff = new MiembroValidador().Validate(staff);
        if (!validacionStaff.IsValid)
        {
            throw new HttpRequestException(validacionStaff.Errors.First().ErrorMessage)
            {
                Data = { { "CodigoError", "400" } }
            };
        }
        staff.Contrasena = BCrypt.HashPassword(staff.Contrasena);
        Persona persona = staff.Persona;

        bool personaExiste = tacosdbContext.Personas.Any(personaBuscar => personaBuscar.Nombre == persona.Nombre &&
             personaBuscar.ApellidoPaterno == persona.ApellidoPaterno &&
             personaBuscar.ApellidoMaterno == persona.ApellidoMaterno ||
             personaBuscar.Email == persona.Email);
        if (personaExiste)
        {
            throw new HttpRequestException()
            {
                Data = { { "CodigoError", "422" } }
            };
        }
        else
        {
            this.tacosdbContext.Personas.Add(persona);
            bool registroExitosoPersona = this.tacosdbContext.SaveChanges() == 1;
            staff.IdPersona = ObtenerIdPersonaRegistrada(persona);
            this.tacosdbContext.Staff.Add(staff);
            bool registroExitosoStaff = this.tacosdbContext.SaveChanges() == 1;
            if (!registroExitosoPersona || !registroExitosoStaff)
            {
                throw new HttpRequestException()
                {
                    Data = { { "CodigoError", "500" } }
                };
            }

        }
    }

    public int ObtenerIdPersonaRegistrada(Persona persona)
    {
        int idPersona = -1;
        var idPersonaObtenida = (from personaBuscar in tacosdbContext.Personas
                                   where personaBuscar.Nombre == persona.Nombre &&
                                   personaBuscar.ApellidoPaterno == persona.ApellidoPaterno &&
                                   personaBuscar.ApellidoMaterno == persona.ApellidoMaterno &&
                                   personaBuscar.Email == persona.Email
                                   select personaBuscar.Id).FirstOrDefault();
        if (idPersonaObtenida != 0)
        {
            idPersona = idPersonaObtenida;
        }
        else
        {
            throw new HttpRequestException()
            {
                Data = { { "CodigoError", "500" } }
            };
        }
        return idPersona;
    }

}
