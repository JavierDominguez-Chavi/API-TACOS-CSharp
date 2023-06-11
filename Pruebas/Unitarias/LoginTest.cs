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

namespace Pruebas.Unitarias;

public class LoginTest
{
    private Uri uri = new Uri("http://localhost:5174");
    private Miembro miembroEsperado = new Miembro()
    {
        Id= 51,
        Contrasena="$2a$11$uuoruMSUPg6k3p7afix30ub2aJuIndaQMvOdQD8rXYaZ2B1xv9vuK",
        PedidosPagados=69,
        IdPersona=117,
        CodigoConfirmacion=0,
        Persona=new Persona()
        {
            Id=117,
            Nombre="Ricardo",
            ApellidoPaterno="Restrepo",
            ApellidoMaterno="Salazar",
            Direccion="calle Pintores #56 Col.Del Rio",
            Email="admin",
            Telefono="2266787890"
        }
    };

    private Credenciales credenciales = new Credenciales
    {
        Email = "admin",
        Contrasena = "ASDFasdf1234",
        EsStaff = false
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
                        this.credenciales
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.True(respuesta.IsSuccessStatusCode);
            credencialesObtenidas = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.NotNull(credencialesObtenidas);
            Assert.Equivalent(this.miembroEsperado, credencialesObtenidas.Miembro);
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_Email()
    {
        this.credenciales.Email="1234";
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        this.credenciales
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, respuesta.StatusCode);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.NotNull(error.Mensaje);
            Assert.True(error.Mensaje.Equals("No se encontró ninguna cuenta con ese email y/o contraseña."));
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_Contrasena()
    {
        this.credenciales.Contrasena="1234";
        using (var clienteHttp = new HttpClient())
        {
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PostAsJsonAsync(
                        "Login",
                        this.credenciales
                    ).Result;
            Assert.NotNull(respuesta);
            Assert.False(respuesta.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, respuesta.StatusCode);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.NotNull (error.Mensaje);
            Assert.True(error.Mensaje.Equals("No se encontró ninguna cuenta con ese email y/o contraseña."));
        }
    }

    [Fact]
    public void IniciarSesion_Fallo_EmailContrasena()
    {
        this.credenciales.Email="oisduyf98";
        this.credenciales.Contrasena="6847468749877";

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
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, respuesta.StatusCode);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.NotNull(error.Mensaje);
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
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, respuesta.StatusCode);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.NotNull(error.Mensaje);
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
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, respuesta.StatusCode);
            var error = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;
            Assert.NotNull(error.Mensaje);
            Assert.True(error.Mensaje.Equals("Todos los campos son obligatorios."));
        }
    }
}