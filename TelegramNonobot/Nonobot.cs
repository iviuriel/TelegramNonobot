using System;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace TelegramNonobot
{
    class Ñoñobot
    {
        private static readonly TelegramBotClient miBot = new TelegramBotClient("1499146557:AAFwfx8t9qM4JsJ7d1Uz9Z8oXUtaBSQBuXs");

        public static void Main(string[] args)
        {
            //Escuchamos los mensajes enviados en los grupos donde esté el bot
            var me = miBot.GetMeAsync().Result;
            Console.Title = "Conectado a bot de Telegram " + me.Username;

            //Asignamos los eventos de lectura de mensajes y captura de errores
            miBot.OnMessage += EventoBotTelegramLeerMensajes;
            miBot.OnReceiveError += EventoBotTelegramErrorRecibido;

            //Iniciamos la lectura de mensajes
            miBot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Escuchando mensajes de @{me.Username}");

            //Si se pulsa INTRO en la consola se detiene la escucha de mensajes
            Console.ReadLine();
            miBot.StopReceiving();
        }

        //Evento que lee los mensajes de los grupos donde esté el bot de Telegram        
        private static async void EventoBotTelegramLeerMensajes(object sender,
            MessageEventArgs eventoArgumentosMensajeRecibido)
        {
            var mensaje = eventoArgumentosMensajeRecibido.Message;

            if (mensaje == null || mensaje.Type != MessageType.Text)
                return;

            Console.WriteLine($"Mensaje de @{mensaje.Chat.Username}:" + mensaje.Text);

            switch (mensaje.Text.Split(' ').First())
            {
                //Según el mensaje leído podremos hacer cualquier tarea
                case "/start":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        $"Hola, soy Ñoñobot{EmojiList.CatWithHearts}. \n\nSi tienes alguien te ha preparado un juego escribe /evento.");
                    break;
                case "/evento":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        $"Así que quieres jugar, ¿eh?. Me encanta jugar {EmojiList.CatWithHearts}. Introduce el código del juego para empezar");
                    break;
                default:
                    break;
            }
        }

        private static void EventoBotTelegramErrorRecibido(object sender,
            ReceiveErrorEventArgs eventoArgumentosErrorRecibidos)
        {
            Console.WriteLine("Error recibido: {0} — {1}",
                eventoArgumentosErrorRecibidos.ApiRequestException.ErrorCode,
                eventoArgumentosErrorRecibidos.ApiRequestException.Message);
        }
    }
}
