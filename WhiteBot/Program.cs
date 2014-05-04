using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AntsBattle;

namespace WhiteBot
{
    class Program
    {
        public static IAntAI WhiteAntAI;
        static void Main(string[] args)
        {
            File.AppendAllText("WhiteBotLogs.txt", "Я родился! \n");
            var path = args[0];
            WhiteAntAI = (IAntAI)Activator.CreateInstance(Assembly.LoadFrom(path)
                        .GetTypes()
                        .First(type => type.GetInterfaces().Any(i => i == typeof(IAntAI))));
            // var world = new World();
            var world = new AIWorld(AntColour.White);
            try
            {
                while (true)
                {
                    var location = (Console.ReadLine() ?? "0;0").Split(';').Select(int.Parse).ToList();
                    File.AppendAllText("WhiteBotLogs.txt", "I read: Answer " + location[0] + ";" + location[1] + "\\n");
                    var a = WhiteAntAI.GetDirection(new Point(location[0], location[1]), world);
                    Console.WriteLine("Answer " + a);
                    File.AppendAllText("WhiteBotLogs.txt", "\nAnswer " + a + "\n");
                }
            }
            catch (Exception e)
            {
                File.AppendAllText("WhiteBotException.txt", e.ToString());
            }
        }
    }
}
