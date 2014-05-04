using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AntsBattle;

namespace BlackBot
{
    class Program
    {
        public static IAntAI BlackAntAI;
        static void Main(string[] args)
        {
            File.AppendAllText("BotLogs.txt", "Я родился! \n");
            var path = args[0];
            BlackAntAI = (IAntAI)Activator.CreateInstance(Assembly.LoadFrom(path)
                        .GetTypes()
                        .First(type => type.GetInterfaces().Any(i => i == typeof(IAntAI))));
            // var world = new World();
            var world = new AIWorld(AntColour.Black);
            try
            {
                while (true)
                {
                    var location = (Console.ReadLine() ?? "0;0").Split(';').Select(int.Parse).ToList();
                    File.AppendAllText("BotLogs.txt", "I read: Answer " + location[0] + ";" + location[1] + "/n");
                    var a = BlackAntAI.GetDirection(new Point(location[0], location[1]), world);
                    Console.WriteLine("Answer " + a);
                    File.AppendAllText("BotLogs.txt", "\nAnswer " + a + "\n");
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("BlackBotException.txt", e.ToString());
            }
        }
    }
}
