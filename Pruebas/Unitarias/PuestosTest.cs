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

namespace Pruebas.Unitarias
{
    public class PuestosTest
    {
        private Uri uri = new Uri("http://localhost:5174");
        private HttpResponseMessage respuestaHttp = new HttpResponseMessage();
        ObservableCollection<Puesto> puestos = new ObservableCollection<Puesto>();

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
        public void ObtenerPuestos_Exito()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
               
                respuestaHttp = clienteHttp.GetAsync("Puestos").Result;
                puestos = respuestaHttp.Content.ReadAsAsync<ObservableCollection<Puesto>>().Result;
                puestos.Count.Should().Be(6);
            }
        }
    }
}
