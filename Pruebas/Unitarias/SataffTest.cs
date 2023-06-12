using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using TACOS.Modelos;
using TACOS.Negocio.PeticionesRespuestas;

namespace Pruebas.Unitarias
{
    public class SataffTest
    {
        private Uri uri = new Uri("http://localhost:5174");
        private HttpResponseMessage respuestaHttp = new HttpResponseMessage();
        private Staff staffPrueba = new Staff()
        {
            Id = 0,
            Contrasena = "asdfASDF1234*gfcasc",
            IdPuesto = 2,
            IdTurno = 1,
            IdPersona = 0,
            Persona = new Persona()
            {
                Id = 0,
                Nombre = "PRUEBA",
                ApellidoPaterno = "PRUEBA",
                ApellidoMaterno = "PRUEBA",
                Direccion = "PRUEBA",
                Email = "prueba@prueba.hotmail",
                Telefono = "2244556677"
            }
        };
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
        public void RegistrarStaff_Exito()
        {
            using (var clienteHttp = new HttpClient())
            {
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff",this.staffPrueba ).Result;
                respuestaHttp.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_NombreInvalido()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Persona.Nombre = "+}..+´+DROP";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "Los nombres sólo pueden contener letras.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_ApellidoPaternoInvalido()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Persona.ApellidoPaterno = "+}..+´+DROP";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "Los nombres sólo pueden contener letras.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_ApellidoMaternoInvalido()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Persona.ApellidoMaterno = "+}..+´+DROP";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "Los nombres sólo pueden contener letras.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_EmailIvalido()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Persona.Email = "+}..+´+DROP";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "El email no tiene el formato correcto.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_TelefonoIvalido()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Persona.Telefono = "+}..+´+DROP";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "El telefono no tiene el formato correcto.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_ContrasenaIvalida()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Contrasena = "hola";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "La contraseña debe tener al menos 8 caracteres: " +
                    "al menos una letra minúscula; al menos una mayúscula y al menos un número";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_EmailExistenteEnElSistema()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Persona.Email = "maledict@proton.me";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "El nombre y/o email que desea registrar ya existe en el sistema.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }

        [Fact]
        public void RegistrarStaff_Fallo_NombreExistenteEnElSistema()
        {
            using (var clienteHttp = new HttpClient())
            {
                this.staffPrueba.Persona.Nombre = "Ricardo";
                this.staffPrueba.Persona.ApellidoMaterno = "Castro";
                this.staffPrueba.Persona.ApellidoPaterno = "Salazar";
                clienteHttp.BaseAddress = this.uri;
                clienteHttp.DefaultRequestHeaders.Authorization = this.GenerarAutenticacion();
                respuestaHttp = clienteHttp.PostAsJsonAsync("Staff", this.staffPrueba).Result;

                string mensajeEsperado = "El nombre y/o email que desea registrar ya existe en el sistema.";
                string jsonContent = respuestaHttp.Content.ReadAsStringAsync().Result;
                dynamic responseObject = JsonConvert.DeserializeObject(jsonContent);
                mensajeEsperado.Should().BeEquivalentTo(responseObject["mensaje"].ToString());
            }
        }
    }
}
