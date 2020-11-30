using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace TelegramNonobot
{
    class ControladorEventos
    {
        private static string src = "./evento.json";

        public static Evento BuildEvento()
        {
            string jsonString = File.ReadAllText(src);
            Evento e = JsonConvert.DeserializeObject<Evento>(jsonString);
            return e;
        }
    }

    public class Evento
    {
        public string Code { get; set; }
        public string Intro { get; set; }
        public Elemento[] Elementos { get; set; }
    }

    public class Elemento
    {
        public string Pista { get; set; }
        public string Respuesta { get; set; }
    }
}
