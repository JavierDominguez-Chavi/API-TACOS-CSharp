using System.Collections.ObjectModel;
using System.Reflection;
using Xunit;
using TACOSMenuAPI.Modelos;
using TACOSMenuAPI.Negocio;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit.Sdk;

namespace Pruebas.Unitarias;

public class MenuTest
{
    private Uri uri = new Uri("http://localhost:5174");

    [Fact]
    public void ObtenerAlimentosSinImagenes_Exito()
    {
        
        ObservableCollection<Alimento> alimentos;
        using (var client = new HttpClient())
        {
            client.BaseAddress = this.uri;
            var respuestaHttp = client.GetAsync("menu").Result;
            respuestaHttp.EnsureSuccessStatusCode();

            var respuesta = respuestaHttp.Content.ReadAsAsync<TACOSMenuAPI.Modelos.Respuesta<List<Alimento>>>().Result;
            Assert.NotNull(respuesta.Datos);
            alimentos = new ObservableCollection<Alimento>(
                respuesta.Datos
            );
            Assert.True( alimentos.Count > 0);
            Assert.NotNull(alimentos.ElementAt(0).Nombre);
            Assert.NotNull(alimentos.ElementAt(1).Nombre);
            Assert.True( alimentos.ElementAt(0).Nombre!.Equals("Orden de bisteck") );
            Assert.True( alimentos.ElementAt(1).Nombre!.Equals("Orden de pastor") );
            foreach (Alimento alimento in alimentos)
            {
                Assert.Null(alimento.Imagen);
            }
        }
    }

    [Fact]
    public void ActualizarExistenciaAlimentos_Exito()
    {
        using (var clienteHttp = new HttpClient())
        { 
            Dictionary<int, int> existenciasAModificar = new Dictionary<int, int>
            {
                { 1, -1},
                { 2, 1},
            };
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PatchAsJsonAsync(
                        "menu",
                        existenciasAModificar
                    ).Result;

            Assert.True(respuesta.IsSuccessStatusCode);

            Dictionary<int,int> nuevasExistencias = new Dictionary < int, int >(
                respuesta.Content.ReadAsAsync<TACOSMenuAPI.Modelos.Respuesta<Dictionary<int,int>>>().Result.Datos!
            );

            Assert.NotNull(nuevasExistencias);
            
        }

    }

    [Fact]
    public void ActualizarExistenciaAlimentos_Fallo_IdAlimentoNoExiste()
    {
        using (var clienteHttp = new HttpClient())
        {
            Dictionary<int, int> existenciasAModificar = new Dictionary<int, int>
            {
                { 420, 69}
            };
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PatchAsJsonAsync(
                        "menu",
                        existenciasAModificar
                    ).Result;

            var error = respuesta.Content.ReadAsAsync<TACOSMenuAPI.Modelos.Respuesta<Dictionary<int,int>>>().Result;
            Assert.True(error.Codigo == ((int)System.Net.HttpStatusCode.NotFound));
            Assert.Contains(error.Mensaje!, $"El alimento solicitado no existe.");
        }
    }

    [Fact]
    public void ActualizarExistenciaAlimentos_Fallo_ExistenciaYaNoPuedeDecrecer()
    {
        using (var clienteHttp = new HttpClient())
        {
            Dictionary<int, int> existenciasAModificar = new Dictionary<int, int>
            {
                { 1, -20000}
            };
            clienteHttp.BaseAddress = this.uri;
            clienteHttp.PatchAsJsonAsync("menu",existenciasAModificar);
            HttpResponseMessage respuesta =
                clienteHttp.PatchAsJsonAsync("menu",existenciasAModificar).Result;

            var error = respuesta.Content.ReadAsAsync<TACOSMenuAPI.Modelos.Respuesta<Dictionary<int,int>>>().Result;
            Assert.True(error.Codigo == ((int)System.Net.HttpStatusCode.Conflict));
            Assert.Contains(error.Mensaje!, "La existencia del alimento solicitado ya no puede decrecer.");
        }
    }

}