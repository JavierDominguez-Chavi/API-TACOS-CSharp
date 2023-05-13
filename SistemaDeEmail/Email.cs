using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDeEmail
{
    public class Email
    {
        public string Asunto { get; set; }
        public string Encabezado { get; set; }
        public string Cuerpo { get; set; }

        public List<string> Destinatarios { get; set; } = new List<string>();
    }
}
