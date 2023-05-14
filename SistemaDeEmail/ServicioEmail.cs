using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeEmail
{
    public class ServicioEmail : IEnviarEmail
    {

        public void EnviarCodigoConfirmacion(string direccion, int codigoConfirmacion)
        {
            Email email = new Email()
            {
                Asunto = "[TACOS] Tu código es: " + codigoConfirmacion,
                Encabezado = "Confirma tu registro a TACOS.",
                Cuerpo = "Tu código de confirmación es: " + codigoConfirmacion,
            };
            email.Destinatarios.Add(direccion);
            this.EnviarEmail(email);
        }
        public void EnviarEmail(Email email)
        {
            string remitente = Properties.CredencialesEmail.Direccion;
            string senderPassword = Properties.CredencialesEmail.Contrasena;
            MailMessage mensaje = new MailMessage();
            mensaje.From = new MailAddress(remitente);
            mensaje.Subject = email.Asunto;
            foreach (string destinatario in email.Destinatarios)
            {
                mensaje.To.Add(destinatario);
            }
            mensaje.IsBodyHtml = true;
            mensaje.Body =
                "<html><body><h1>" + email.Encabezado + "</h1>" + email.Cuerpo + "</body></html>";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(remitente, senderPassword),
                EnableSsl = true
            };
            smtpClient.Send(mensaje);
        }
    }
}
