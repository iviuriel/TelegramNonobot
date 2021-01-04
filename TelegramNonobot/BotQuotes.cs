using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramNonobot
{
    class BotQuotes
    {
        public static readonly string Start = $"Hola, soy Argi. \n\nSi tienes alguien te ha preparado un juego escribe /event.";
        public static readonly string StartInEvent = "Estás jugando ahora mismo. Si quieres salir, escribe /close.";

        public static readonly string Close = "Pero bueno, si todavía no has empezado a jugar.";
        public static readonly string CloseInEvent = $"Pues nada el juego ha terminado... {EmojiList.SadFace}";

        public static readonly string Event = $"Así que quieres jugar, ¿eh?. Me encanta jugar {EmojiList.CatWithHearts}. Para empezar a jugar introduce el código después del comando. Mira se hace así: '/event eXXXXXXXXX'";
        public static readonly string EventInEvent = "Oye oye que ya estás en un evento, si quieres entrar a otro sal de este primero. Para salir, escribe /close.";

        public static readonly string WrongCode = "Lo siento, no existe un evento con ese código.";
        public static readonly string NotValidCode = "El código introducido no es un código de evento válido. Los códigos de evento son del tipo eXXXXXXXXX";

        public static readonly string CorrectAnswer = "¡Clin, clin, clin! Respuesta correcta. Siguiente pista:";
        public static readonly string WrongAnswer = "Nooooo... Esa no es... Si necesitas que te repita la pista escribe /hint";
        public static readonly string CompletedEvent = "¡Enhorabuena!¡Has completado el juego! Espero volver a verte pronto y gracias por jugar.";

        public static readonly string Help = "Yo puedo ayudarte dandote pistas y a jugar gymkhanas. Puedes controlarme con estos comandos.\n\n/event - para empezar una ghymkana\n\n**En una juego**\n/close - termina el juego activo\n/hint - repite la última pista";
    }
}
