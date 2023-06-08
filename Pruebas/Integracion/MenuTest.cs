using System.Collections.ObjectModel;
using System.Reflection;
using Xunit;
using TACOSMenuAPI.Modelos;
using TACOSMenuAPI.Negocio;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using Xunit.Sdk;

namespace Pruebas.Integracion;

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
            var response = client.GetAsync("menu").Result;
            response.EnsureSuccessStatusCode();

            alimentos = new ObservableCollection<Alimento>(
                response.Content.ReadAsAsync<TACOSMenuAPI.Modelos.Respuesta<List<Alimento>>>().Result.Datos
            );
            Assert.True( alimentos.Count > 0);
            Assert.True( alimentos.ElementAt(0).Nombre.Equals("Orden de bisteck") );
            Assert.True( alimentos.ElementAt(1).Nombre.Equals("Orden de pastor") );
            foreach (Alimento alimento in alimentos)
            {
                Assert.Null(alimento.Imagen);
            }
        }
    }

    [Fact]
    public void ActualizarExistenciaAlimentos_Exito()
    {
        using (var contexto = new TacosdbContext())
        using (var clienteHttp = new HttpClient())
        { 
            Alimento bisteck = contexto.Alimentos.FirstOrDefault(a => a.Id==2);
            int bisteckExistenciaActual = (int)bisteck.Existencia;
            Alimento pastor = contexto.Alimentos.FirstOrDefault(a => a.Id==1);
            int pastorExistenciaActual = (int)pastor.Existencia;
            Dictionary<int, int> existenciasAModificar = new Dictionary<int, int>
            {
                { bisteck.Id, -1},
                { pastor.Id, 1},
            };
            clienteHttp.BaseAddress = this.uri;
            HttpResponseMessage respuesta =
                    clienteHttp.PatchAsJsonAsync(
                        "menu",
                        existenciasAModificar
                    ).Result;

            Assert.True(respuesta.IsSuccessStatusCode);

            Dictionary<int,int> nuevasExistencias = new Dictionary < int, int >(
                respuesta.Content.ReadAsAsync<TACOSMenuAPI.Modelos.Respuesta<Dictionary<int,int>>>().Result.Datos
            );

            Assert.NotNull(nuevasExistencias);

            Assert.True(nuevasExistencias[bisteck.Id] == bisteckExistenciaActual-1);
            Assert.True(nuevasExistencias[pastor.Id] == pastorExistenciaActual+1);

            
            existenciasAModificar[bisteck.Id] = 1;
            existenciasAModificar[pastor.Id] = -1;
            clienteHttp.PatchAsJsonAsync("menu",existenciasAModificar);
            
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
            Assert.True(error.Mensaje.Contains($"El alimento solicitado no existe."));
        }
    }

    [Fact]
    public void ActualizarExistenciaAlimentos_Fallo_ExistenciaYaNoPuedeDecrecer()
    {
        using (var clienteHttp = new HttpClient())
        {
            Dictionary<int, int> existenciasAModificar = new Dictionary<int, int>
            {
                { 1, -20}
            };
            clienteHttp.BaseAddress = this.uri;
            clienteHttp.PatchAsJsonAsync("menu",existenciasAModificar);
            HttpResponseMessage respuesta =
                clienteHttp.PatchAsJsonAsync("menu",existenciasAModificar).Result;

            var error = respuesta.Content.ReadAsAsync<TACOSMenuAPI.Modelos.Respuesta<Dictionary<int,int>>>().Result;
            Assert.True(error.Codigo == ((int)System.Net.HttpStatusCode.Conflict));
            Assert.True(error.Mensaje.Contains("La existencia del alimento solicitado ya no puede decrecer."));
        }
    }
}