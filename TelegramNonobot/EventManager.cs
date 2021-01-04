using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace TelegramNonobot
{
    class EventManager
    {
        private static string srcEvents = Path.Combine(Nonobot.GetProjectDirectory(), @"Events\", "{0}.json");
        private static Dictionary<int, Event> userEvents = new Dictionary<int, Event>();

        public static readonly string FINISH_TEXT = "FINISH";

        public static bool SetEvent(string code, int id)
        {
            string path = String.Format(srcEvents, code);

            if (File.Exists(path))
            {
                string jsonString = File.ReadAllText(path);
                Event e = JsonConvert.DeserializeObject<Event>(jsonString);
                if (e.Code == code)
                {
                    userEvents.Add(id, e);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }           
        }

        public static bool IsUserPlaying(int id)
        {
            return userEvents.ContainsKey(id);
        }

        public static string GetIntroText(int id)
        {
            if (IsUserPlaying(id))
            {
                return userEvents[id].Intro;
            }
            else
            {
                Console.WriteLine("[ERROR] El usuario con id " + id + " no está jugando y no puede iniciar el texto");
                return null;
            }
        } 

        public static string GetNextHint(int id)
        {
            if (IsUserPlaying(id))
            {
                Event e = userEvents[id];
                int hintIndex = e.FirstNotCompletedHint();
                if(hintIndex != -1)
                {
                    int h = hintIndex+1;
                    return h+".- "+e.Elements[hintIndex].Hint; //Formato: 1.- PISTA PISTA PISTA
                }
                else
                {
                    return FINISH_TEXT;
                }
            }
            else
            {
                Console.WriteLine("[ERROR] El usuario con id "+id+" no está jugando y no puede pedir una pista");
                return null;
            }
        }

        public static bool CheckAnswer(string answer, int id)
        {
            if (IsUserPlaying(id))
            {
                Event e = userEvents[id];
                int hintIndex = e.FirstNotCompletedHint();
                if (hintIndex != -1)
                {
                    if(e.Elements[hintIndex].Answer == answer)
                    {
                        e.Elements[hintIndex].Completed = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }                   
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Console.WriteLine("[ERROR] El usuario con id " + id + " no está jugando y no puede comprobar una respuesta");
                return false;
            }
        }

        public static void FinishEvent(int id)
        {
            userEvents.Remove(id);
        }
    }

    public class Event
    {
        public string Code { get; set; }
        public string Intro { get; set; }
        public Element[] Elements { get; set; }

        public int FirstNotCompletedHint()
        {
            int index = -1;
            for(int i = 0; i<Elements.Count(); i++)
            {
                if (!Elements[i].Completed)
                {
                    index = i;
                    break;
                }
            }

            return index;
        }
    }

    public class Element
    {
        public string Hint { get; set; }
        public string Answer { get; set; }
        public bool Completed { get; set; }

        public Element() { this.Completed = false; }
    }
}
