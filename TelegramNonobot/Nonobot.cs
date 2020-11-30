using System;
using System.Linq;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

namespace TelegramNonobot
{
    class Nonobot
    {
        private static readonly TelegramBotClient miBot = new TelegramBotClient("1499146557:AAFwfx8t9qM4JsJ7d1Uz9Z8oXUtaBSQBuXs");


        public static void Main(string[] args)
        {
            //Escuchamos los mensajes enviados en los grupos donde esté el bot
            var me = miBot.GetMeAsync().Result;
            Console.Title = "Conectado a bot de Telegram " + me.Username;

            //Asignamos los eventos de lectura de mensajes y captura de errores
            miBot.OnMessage += BotReceiveMessage;
            miBot.OnReceiveError += BotReceiveError;

            //Iniciamos la lectura de mensajes
            miBot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Escuchando mensajes de @{me.Username}");

            //Si se pulsa INTRO en la consola se detiene la escucha de mensajes
            Console.ReadLine();
            miBot.StopReceiving();
        }

        //Evento que lee los mensajes de los grupos donde esté el bot de Telegram        
        private static void BotReceiveMessage(object sender,
            MessageEventArgs eventArgsMessage)
        {
            var mensaje = eventArgsMessage.Message;

            if (mensaje == null || mensaje.Type != MessageType.Text)
                return;

            Console.WriteLine($"Mensaje de @{mensaje.Chat.Username}:" + mensaje.Text);

            if (EventManager.IsUserPlaying(mensaje.From.Id)) //Si está jugando un evento
            {
                PlayEvent(mensaje);
            }
            else
            {
                UseCommand(mensaje);
            }

            
        }

        private static void BotReceiveError(object sender,
            ReceiveErrorEventArgs eventoArgumentosErrorRecibidos)
        {
            Console.WriteLine("Error recibido: {0} — {1}",
                eventoArgumentosErrorRecibidos.ApiRequestException.ErrorCode,
                eventoArgumentosErrorRecibidos.ApiRequestException.Message);
        }

        private static async void PlayEvent(Message mensaje)
        {
            var answer = mensaje.Text.Split(' ').First();
            switch (answer)
            {
                //Según el mensaje leído podremos hacer cualquier tarea
                case "/start":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        $"Estás jugando ahora mismo. Si quieres salir, escribe /close.");
                    break;
                case "/close":
                    EventManager.FinishEvent(mensaje.From.Id);
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        $"Pues nada el juego ha terminado... {EmojiList.SadFace}");
                    break;
                case "/help":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        "Help");
                    break;
                case "/evento":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        "Oye oye que ya estás en un evento, si quieres entrar a otro sal de este primero. Para salir, escribe /close.");
                    break;
                case "/hint":
                    //Enviamos la siguiente pista
                    string hint = EventManager.GetNextHint(mensaje.From.Id);
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id, hint);
                    break;
                default:
                    bool correctAnswer = EventManager.CheckAnswer(answer, mensaje.From.Id);
                    if (correctAnswer)
                    {
                        await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                            "¡Clin, clin, clin! Respuesta correcta. Siguiente pista:");
                        //Enviamos la siguiente pista
                        string h = EventManager.GetNextHint(mensaje.From.Id);
                        //Si no es el texto de finalizado le da la siguiente pista
                        if (h != EventManager.FINISH_TEXT)
                        {
                            await miBot.SendTextMessageAsync(mensaje.Chat.Id, h);
                        }
                        else
                        {
                            EventManager.FinishEvent(mensaje.From.Id);
                            //Si se ha acabado el juego, notificamos al usuario que ha ganado
                            await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                                $"¡Enhorabuena {mensaje.From.Username}!¡Has completado el juego! Espero volver a verte pronto y gracias por jugar.");
                        }
                    }
                    else
                    {
                        await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                            "Nooooo... Esa no es... Si necesitas que te repita la pista escribe /hint");
                    }
                    break;
            }
        }

        private static async void UseCommand(Message mensaje)
        {
            switch (mensaje.Text.Split(' ').First())
            {
                //Según el mensaje leído podremos hacer cualquier tarea
                case "/start":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        $"Hola, soy Ñoñobot{EmojiList.CatWithHearts}. \n\nSi tienes alguien te ha preparado un juego escribe /evento.");
                    break;
                case "/close":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        $"Pero bueno, {mensaje.From.Username}, si todavía no has empezado a jugar.");
                    break;
                case "/help":
                    await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                        $"Help");
                    break;
                case "/evento":
                    //Si el mensaje viene en dos partes obtenemos la segunda como código
                    if (mensaje.Text.Split(' ').Count() > 1)
                    {
                        CheckCode(mensaje);
                    }
                    else
                    {
                        await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                            $"Así que quieres jugar, ¿eh?. Me encanta jugar {EmojiList.CatWithHearts}. Para empezar a jugar introduce el código después del comando. Mira se hace así: '/evento eXXXXXXXXX'");
                    }
                    break;
                default:
                    break;
            }
        }

        private static async void CheckCode(Message m)
        {
            var code = m.Text.Split(' ')[1];

            //El codigo debe empezar por e y tene una longitud de 10 caracteres para ser valido
            if (code.StartsWith("e") && code.Count() == 10)
            {
                //Se activa el codigo del evento y se devuelve si ha sido correcta para enviar el mensaje adecuado
                bool correct = ActivateEvent(code, m.From.Id);
                if (correct)
                {
                    await miBot.SendTextMessageAsync(m.Chat.Id, "Código correcto!");
                    //Obtenemos el texto de inicio y lo envíamos
                    string intro = EventManager.GetIntroText(m.From.Id);
                    await miBot.SendTextMessageAsync(m.Chat.Id, intro);
                    //Enviamos la primera pista
                    string hint = EventManager.GetNextHint(m.From.Id);
                    await miBot.SendTextMessageAsync(m.Chat.Id, hint);
                }
                else
                {
                    await miBot.SendTextMessageAsync(m.Chat.Id, "Lo siento, no existe un evento con ese código.");
                }
            }
            else
            {
                await miBot.SendTextMessageAsync(m.Chat.Id, "El código introducido no es un código de evento válido. Los códigos de evento son del tipo eXXXXXXXXX");
            }

        }

        private static bool ActivateEvent(string code, int idUsuario){ return EventManager.SetEvent(code, idUsuario);}

        
    }
}
