using System.Collections.ObjectModel;
using System.Reflection;
using Xunit;
using TACOSMenuAPI.Modelos;
using TACOSMenuAPI.Negocio;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit.Sdk;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TACOS.Modelos;
using TACOS.Negocio.PeticionesRespuestas;

namespace Pruebas;

public class LoginTest
{
    private Uri uri = new Uri("http://localhost:5174");
    private Miembro miembroEsperado = new Miembro()
    {
        Id= 61,
        Contrasena="$2a$11$mNY8fms2cPVBvw32T2eYbeTkel4N28C9B45GYlH509SuA/gxHwcY2",
        PedidosPagados=0,
        IdPersona=105,
        CodigoConfirmacion=0,
        Persona=new Persona()
        {
            Id=105,
            Nombre="hjg",
            ApellidoPaterno="jhg",
            ApellidoMaterno="jhg",
            Direccion="jh",
            Email="maledict@proton.me",
            Telefono="gjhg"
        }
    };

    [Fact]
    public void IniciarSesion_Exito()
    {
        RespuestaCredenciales credencialesObtenidas = new RespuestaCredenciales();
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        new { Email = "maledict@proton.me", Contrasena = "asdf"}
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.True(respuesta.IsSuccessStatusCode);
            credencialesObtenidas = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.NotNull(credencialesObtenidas);
            Assert.Equivalent(miembroEsperado, credencialesObtenidas.Miembro);
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_Email()
    {
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        new { Email = "1234", Contrasena = "asdf"}
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(respuesta.StatusCode, System.Net.HttpStatusCode.Unauthorized);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.True(error.Mensaje.Equals("No se encontró ninguna cuenta con ese email y/o contraseña."));
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_Contrasena()
    {
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        new { Email = "maledict@proton.me", Contrasena = "1234"}
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(respuesta.StatusCode, System.Net.HttpStatusCode.Unauthorized);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.True(error.Mensaje.Equals("No se encontró ninguna cuenta con ese email y/o contraseña."));
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_EmailContrasena()
    {
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        new { Email = "dsfghkjsdfhgjkfsdh", Contrasena = "1234"}
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(respuesta.StatusCode, System.Net.HttpStatusCode.Unauthorized);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.True(error.Mensaje.Equals("No se encontró ninguna cuenta con ese email y/o contraseña."));
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_EmailVacio()
    {
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        new { Email = "", Contrasena = "1234"}
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(respuesta.StatusCode, System.Net.HttpStatusCode.BadRequest);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.True(error.Mensaje.Equals("Todos los campos son obligatorios."));
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_ContrasenaVacia()
    {
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        new { Email = "maledict@proton.me", Contrasena = ""}
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(respuesta.StatusCode, System.Net.HttpStatusCode.BadRequest);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.True(error.Mensaje.Equals("Todos los campos son obligatorios."));
        }
    }
}