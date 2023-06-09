using System.Collections.ObjectModel;
using System.Reflection;
using Xunit;
using TACOSMenuAPI.Negocio;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit.Sdk;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TACOS.Modelos;
using FluentAssertions;
using TACOS.Negocio.PeticionesRespuestas;

namespace Pruebas.Unitarias;

public class MiembroTest
{
    private Uri uri = new Uri("http://localhost:5174");
    private Miembro miembroPrueba = new Miembro()
    {
        Id= 0,
        Contrasena="PRUEBA",
        PedidosPagados=0,
        IdPersona=0,
        CodigoConfirmacion=0,
        Persona=new Persona()
        {
            Id=0,
            Nombre="PRUEBA",
            ApellidoPaterno="PRUEBA",
            ApellidoMaterno="PRUEBA",
            Direccion="PRUEBA",
            Email="vitocfdz@proton.me",
            Telefono="123"
        }
    };

    [Fact]
    public void RegistrarMiembro_Exito()
    {
        using (var contexto = new TacosdbContext())
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "miembro",
                        this.miembroPrueba
                    ).Result;

            Assert.NotNull(respuesta);
            Assert.True(respuesta.IsSuccessStatusCode);

            Miembro miembroObtenido = respuesta.Content.ReadAsAsync<Respuesta<Miembro>>().Result.Datos;

            Assert.NotNull(miembroObtenido);
            Assert.NotNull(miembroObtenido.Persona);

            miembroObtenido.Should().BeEquivalentTo
            (
                this.miembroPrueba,
                options =>
                    options.Excluding(miembro => miembro.Id)
                           .Excluding(miembro => miembro.IdPersona)
                           .Excluding(miembro => miembro.Persona!.Id)
                           .Excluding(miembro => miembro.CodigoConfirmacion)
                           .Excluding(miembro => miembro.Contrasena)
            );

            Assert.True(miembroObtenido.CodigoConfirmacion > 0);
            Assert.False(String.IsNullOrWhiteSpace(miembroObtenido.Contrasena));
            Assert.NotEqual(miembroObtenido.Contrasena, this.miembroPrueba.Contrasena);
            Assert.Equal(miembroObtenido.IdPersona, miembroObtenido.Persona!.Id);

            contexto.Miembros.Remove(miembroObtenido);
            contexto.Personas.Remove(miembroObtenido.Persona);
            contexto.SaveChanges();
        }
    }

    [Fact]
    public void RegistrarMiembro_Fallo_EmailRepetido()
    {
        using (var clienteHttp = new HttpClient())
        {
            this.miembroPrueba.Persona!.Email="maledict@proton.me";
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "miembro",
                        this.miembroPrueba
                    ).Result;

            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.UnprocessableEntity, respuesta.StatusCode);

            var error = respuesta.Content.ReadAsAsync < RespuestaCredenciales >().Result;

            Assert.NotNull(error);
            Assert.True(error.Mensaje!.Equals("Ya existe una cuenta con este email."));
        }
    }

    [Fact]
    public void ConfirmarRegistro_Exito()
    {
        using (var contexto = new TacosdbContext())
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuestaRegistro =
                clienteHttp.PostAsJsonAsync("miembro",this.miembroPrueba).Result;
            Miembro miembroRegistrado = respuestaRegistro.Content
                                                         .ReadAsAsync<Respuesta<Miembro>>()
                                                         .Result
                                                         .Datos;
            HttpResponseMessage respuestaConfirmacion =
                clienteHttp.PutAsJsonAsync("miembro",miembroRegistrado).Result;

            Assert.NotNull(respuestaConfirmacion);
            Assert.True(respuestaConfirmacion.IsSuccessStatusCode);

            Miembro miembroConfirmado = respuestaConfirmacion.Content
                                                             .ReadAsAsync<Respuesta<Miembro>>()
                                                             .Result
                                                             .Datos;
            Assert.NotNull(miembroConfirmado);
            Assert.True(miembroConfirmado.CodigoConfirmacion==0);
            Assert.NotEqual(miembroRegistrado.CodigoConfirmacion, miembroConfirmado.CodigoConfirmacion);

            contexto.Miembros.Remove(miembroConfirmado);
            contexto.Personas.Remove(miembroConfirmado.Persona!);
            contexto.SaveChanges();
        }
    }

    [Fact]
    public void ConfirmarRegistro_Fallo_IdMiembroIncorrecto()
    {
        using (var contexto = new TacosdbContext())
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuestaRegistro =
                clienteHttp.PostAsJsonAsync("miembro",this.miembroPrueba).Result;
            Miembro miembroRegistrado = respuestaRegistro.Content
                                                         .ReadAsAsync<Respuesta<Miembro>>()
                                                         .Result
                                                         .Datos;
            int idOriginal = miembroRegistrado.Id;
            miembroRegistrado.Id = 654654654;

            HttpResponseMessage respuestaConfirmacion =
                clienteHttp.PutAsJsonAsync("miembro",miembroRegistrado).Result;

            Assert.NotNull(respuestaConfirmacion);
            Assert.Equal(System.Net.HttpStatusCode.NotFound, respuestaConfirmacion.StatusCode);

            var error = respuestaConfirmacion.Content.ReadAsAsync<Respuesta<Miembro>>().Result;

            Assert.NotNull(error);
            Assert.Equal("No se encontró el miembro solicitado.", error.Mensaje);

            miembroRegistrado.Id = idOriginal;
            contexto.Miembros.Remove(miembroRegistrado);
            contexto.Personas.Remove(miembroRegistrado.Persona!);
            contexto.SaveChanges();
        }
    }

    [Fact]
    public void ConfirmarRegistro_Fallo_CodigoIncorrecto()
    {
        using (var contexto = new TacosdbContext())
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuestaRegistro =
                clienteHttp.PostAsJsonAsync("miembro",this.miembroPrueba).Result;
            Miembro miembroRegistrado = respuestaRegistro.Content
                                                         .ReadAsAsync<Respuesta<Miembro>>()
                                                         .Result
                                                         .Datos;
            int codigoOriginal = (int)miembroRegistrado.CodigoConfirmacion;
            miembroRegistrado.CodigoConfirmacion = -1;

            HttpResponseMessage respuestaConfirmacion =
                clienteHttp.PutAsJsonAsync("miembro",miembroRegistrado).Result;

            Assert.NotNull(respuestaConfirmacion);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, respuestaConfirmacion.StatusCode);

            var error = respuestaConfirmacion.Content.ReadAsAsync<Respuesta<Miembro>>().Result;

            Assert.NotNull(error);
            Assert.Equal("El código es incorrecto.", error.Mensaje);

            miembroRegistrado.CodigoConfirmacion = codigoOriginal;
            contexto.Miembros.Remove(miembroRegistrado);
            contexto.Personas.Remove(miembroRegistrado.Persona!);
            contexto.SaveChanges();
        }
    }
}