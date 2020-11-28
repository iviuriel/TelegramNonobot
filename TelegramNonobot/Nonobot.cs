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
        private static Dictionary<int, int> userStates = new Dictionary<int, int>();

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
                    //Si el mensaje viene en dos partes obtenemos la segunda como código
                    if (mensaje.Text.Split(' ').Count() > 1)
                    {
                        ControlEventos(mensaje, false);
                    }
                    else
                    {
                        userStates.Add(mensaje.From.Id, 0);
                        await miBot.SendTextMessageAsync(mensaje.Chat.Id,
                            $"Así que quieres jugar, ¿eh?. Me encanta jugar {EmojiList.CatWithHearts}. Introduce el código del juego para empezar");
                    }                    
                    break;
                default:
                    if (userStates.ContainsKey(mensaje.From.Id))
                    {
                        ControlEventos(mensaje, true);
                    }
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

        private static async void ControlEventos(Message m, bool onlyCode)
        {
            var code = "";
            //Si se especifica que viene el codigo sin comando se coge la primera separación y si viene con evento es después del espacio
            if (onlyCode)
            {
                code = m.Text.Split(' ').First();           
            }
            else
            {
                code = m.Text.Split(' ')[1];
            }

            //El codigo debe empezar por e y tene una longitud de 10 caracteres para ser valido
            if (code.StartsWith("e") && code.Count() == 10)
            {
                //Se activa el codigo del evento y se devuelve si ha sido correcta para enviar el mensaje adecuado
                bool correct = ActivarEvento(code);
                if (correct)
                {
                    await miBot.SendTextMessageAsync(m.Chat.Id, "Código correcto!");
                    userStates[m.From.Id] = 1;
                    Console.WriteLine($"El estado del usuario con id {m.From.Id} es 1");
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

        private static bool ActivarEvento(string code)
        {
            //Si el usuario está esperando el evento
           
                
            return true;
        }
    }
}
