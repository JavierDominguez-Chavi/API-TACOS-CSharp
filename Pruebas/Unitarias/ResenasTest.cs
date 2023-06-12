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
using Newtonsoft.Json;

namespace Pruebas.Unitarias
{
    public class ResenasTest
    {
        private Uri uri = new Uri("http://localhost:5174");
        private HttpResponseMessage respuestaHttp = new HttpResponseMessage();
        ObservableCollection<Resena> resenas = new ObservableCollection<Resena>();

        private AuthenticationHeaderValue GenerarAutenticacion()
        {
            AuthenticationHeaderValue autenticacion;
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = this.uri;
                HttpResponseMessage respuesta =
                    cliente.PostAsJsonAsync("Login", new { Email = "maledict@proton.me", Contrasena = "asdf", EsStaff = true })
                    .Result;
                var credencialesObtenidas = respuesta.Content.ReadAsAsync<RespuestaCredenciales>().Result;

                autenticacion = new AuthenticationHeaderValue("Bearer", credencialesObtenidas.Token);
            }
            return autenticacion;
        }


        [Fact]
        public void ObtenerResenas_Exito()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Add("User-Agent", "Anything");

                respuestaHttp = clienteHttp.GetAsync("Resenas").Result;
                resenas = respuestaHttp.Content.ReadAsAsync<ObservableCollection<Resena>>().Result;
                resenas.Count.Should().Be(3);
            }
        }


        //Para este caso de prueba es necesario que la conexión con el servidor de la base de datos sea nula
        [Fact]
        public void ObtenerResenas_FalloErrorDeServidor()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Add("User-Agent", "Anything");

                respuestaHttp = clienteHttp.GetAsync("Resenas").Result;

                respuestaHttp.StatusCode.Should().Be(System.Net.HttpStatusCode.InternalServerError);
            }
        }


        //Como los id son llaves primarias autoincrementables es recomandable insertar un registro en la base de datos
        //Seleccionar el id del registro ingresado o bien seleccionar uno de los existentes
        [Fact]
        public void BorrarResenas_Exito()
        {
            using (var clienteHttp = new HttpClient())
            {
                int idResenaEliminar = 19;
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsync($"Resenas?idResena={idResenaEliminar}", null).Result;

                respuestaHttp.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            }
        }

        [Fact]
        public void BorrarResenas_Fallo_IdResenaInexistente()
        {
            using (var clienteHttp = new HttpClient())
            {
                int idResenaEliminar = -090919;
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsync($"Resenas?idResena={idResenaEliminar}", null).Result;
                string mensajeEsperado = "Ningun registro coincide con la reseña que desea eliminar.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void BorrarResenas_Fallo_IdResenaInvalido()
        {
            using (var clienteHttp = new HttpClient())
            {
                int idResenaEliminar = 0;
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsync($"Resenas?idResena={idResenaEliminar}", null).Result;
                string mensajeEsperado = "Se requiere un ID de reseña válido para eliminar el registro.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void BorrarResenas_Fallo_NoAutorizado()
        {
            using (var clienteHttp = new HttpClient())
            {
                int idResenaEliminar = 19;
                clienteHttp.BaseAddress = this.uri;
                //clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsync($"Resenas?idResena={idResenaEliminar}", null).Result;

                respuestaHttp.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
            }
        }

    }
}
