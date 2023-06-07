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
using Newtonsoft.Json;
using TACOS.Negocio.PeticionesRespuestas;

namespace Pruebas;

public class PedidoTest
{
    private Uri uri = new Uri("http://localhost:5174");
    private Pedido pedidoPrueba = new Pedido()
    {
        Fecha = DateTime.Now,
        IdMiembro=61,
        Total = 420.69,
        Estado = 1,
    };

    [Fact]
    public void RegistrarPedido_Exito()
    {
        using (var contexto = new TacosdbContext())
        using (var cliente = new HttpClient())
        {
            this.pedidoPrueba.Alimentospedidos =
                new List<Alimentospedido>
                {
                    new Alimentospedido { IdAlimento = 1, Cantidad = 420 },
                    new Alimentospedido { IdAlimento = 2, Cantidad = 69 },
                };

            cliente.BaseAddress = this.uri;
            cliente.DefaultRequestHeaders.Authorization = ObtenerAutenticacion();
            HttpResponseMessage respuestaPedido =
                cliente.PostAsJsonAsync("pedidos",pedidoPrueba).Result;

            Assert.NotNull(respuestaPedido);
            Assert.True(respuestaPedido.IsSuccessStatusCode);
            Pedido pedidoRegistrado = respuestaPedido.Content.ReadAsAsync<Respuesta<Pedido>>().Result.Datos;
            Assert.NotNull(pedidoRegistrado);
            Assert.True(pedidoRegistrado.Id > 0);
            Assert.True(pedidoRegistrado.Alimentospedidos.Count == 2);

            foreach (Alimentospedido alimento in pedidoRegistrado.Alimentospedidos)
            {
                contexto.Alimentospedidos.Remove(alimento);
            }
            contexto.Pedidos.Remove(pedidoRegistrado);
            Assert.True(contexto.SaveChanges() > 0);
        }
    }

    [Fact]
    public void RegistrarPedido_Fallo_EstadoInvalido()
    {
        using (var contexto = new TacosdbContext())
        using (var cliente = new HttpClient())
        {
            this.pedidoPrueba.Alimentospedidos =
                new List<Alimentospedido>
                {
                    new Alimentospedido { IdAlimento = 1, Cantidad = 420 },
                    new Alimentospedido { IdAlimento = 2, Cantidad = 69 },
                };
            this.pedidoPrueba.Estado = 69;

            cliente.BaseAddress = this.uri;
            cliente.DefaultRequestHeaders.Authorization = ObtenerAutenticacion();
            HttpResponseMessage respuestaPedido =
                cliente.PostAsJsonAsync("pedidos",pedidoPrueba).Result;

            Assert.NotNull(respuestaPedido);
            Assert.False(respuestaPedido.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, respuestaPedido.StatusCode);
        }
    }

    [Fact]
    public void RegistrarPedido_Fallo_SinAlimentos()
    {
        using (var contexto = new TacosdbContext())
        using (var cliente = new HttpClient())
        {
            cliente.BaseAddress = this.uri;
            cliente.DefaultRequestHeaders.Authorization = ObtenerAutenticacion();
            HttpResponseMessage respuestaPedido =
                cliente.PostAsJsonAsync("pedidos",pedidoPrueba).Result;

            Assert.NotNull(respuestaPedido);
            Assert.False(respuestaPedido.IsSuccessStatusCode);
            var error = respuestaPedido.Content.ReadAsAsync < Respuesta < Pedido > >().Result;
            Assert.NotNull(error);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest,respuestaPedido.StatusCode);
            Assert.Equal("El pedido debe contener al menos un alimento, " +
                         "y todos los alimentos deben tener una cantidad mayor a 0.",
                         error.Mensaje);
        }
    }

    [Fact]
    public void RegistrarPedido_Fallo_AlimentosCantidadesInvalidas1()
    {
        using (var contexto = new TacosdbContext())
        using (var cliente = new HttpClient())
        {
            this.pedidoPrueba.Alimentospedidos =
                new List<Alimentospedido>
                {
                    new Alimentospedido { IdAlimento = 1, Cantidad = 0 },
                    new Alimentospedido { IdAlimento = 2, Cantidad = 69 },
                };

            cliente.BaseAddress = this.uri;
            cliente.DefaultRequestHeaders.Authorization = ObtenerAutenticacion();
            HttpResponseMessage respuestaPedido =
                cliente.PostAsJsonAsync("pedidos",pedidoPrueba).Result;

            Assert.NotNull(respuestaPedido);
            Assert.False(respuestaPedido.IsSuccessStatusCode);
            var error = respuestaPedido.Content.ReadAsAsync < Respuesta < Pedido > >().Result;
            Assert.NotNull(error);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, respuestaPedido.StatusCode);
            Assert.Equal("El pedido debe contener al menos un alimento, " +
                         "y todos los alimentos deben tener una cantidad mayor a 0.",
                         error.Mensaje);
        }
    }

    [Fact]
    public void RegistrarPedido_Fallo_NoAutorizado()
    {
        using (var contexto = new TacosdbContext())
        using (var cliente = new HttpClient())
        {
            this.pedidoPrueba.Alimentospedidos =
                new List<Alimentospedido>
                {
                    new Alimentospedido { IdAlimento = 1, Cantidad = 0 },
                    new Alimentospedido { IdAlimento = 2, Cantidad = 69 },
                };

            cliente.BaseAddress = this.uri;
            //cliente.DefaultRequestHeaders.Authorization = ObtenerAutenticacion();
            HttpResponseMessage respuestaPedido =
                cliente.PostAsJsonAsync("pedidos",pedidoPrueba).Result;

            Assert.NotNull(respuestaPedido);
            Assert.False(respuestaPedido.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, respuestaPedido.StatusCode);
        }
    }

    private AuthenticationHeaderValue ObtenerAutenticacion()
    {
        AuthenticationHeaderValue autenticacion;
        using (var cliente = new HttpClient())
        {
            cliente.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                cliente.PostAsJsonAsync("Login",new { Email = "maledict@proton.me", Contrasena = "asdf"}).Result;
            var credencialesObtenidas = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;

            autenticacion =new AuthenticationHeaderValue("Bearer", credencialesObtenidas.Token);
        }
        return autenticacion;
    }
}