using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntsBattle
{
    public enum AntColour
    {
        Black, White, None
    }

    public class BotInitialisation
    {
        private readonly string _path;
        private readonly string _fileName;
        private Process _process;
        private readonly int _ind;
        private readonly World _world;
        public BotInitialisation(string path, string fileName, int ind, World world)
        {
            _path = path;
            _fileName = fileName;
            _ind = ind;
            _world = world;
        }

        public void Start()
        {
            StartProcess(_path);
        }

        private AntColour ConvertObjectInEnum(string answer)
        {
            switch (answer)
            {
                case "Black":
                    return AntColour.Black;
                case "White":
                    return AntColour.White;
                default:
                    return AntColour.None;
            }
        }

        private void StartProcess(string path)
        {
            _process = new Process();
            _process.StartInfo.FileName = _fileName;
            _process.StartInfo.Arguments = path;
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.CreateNoWindow = false;
            //_process.StartInfo.CreateNoWindow = true;
            _process.Start();
            while (true)
            {
                if (_ind == _world.GetTurn())
                {
                    _process.StandardInput.WriteLine(_world.GetLocation().X + ";" + _world.GetLocation().Y);
                    File.AppendAllText("ServerLogs" + _ind + ".txt", _world.GetLocation().X + ";" + _world.GetLocation().Y + "\n");
                    while (true)
                    {
                        var answer = (_process.StandardOutput.ReadLine() ?? "").Split(' ').ToList();
                        
                        if (answer[0] == "GetObject")
                        {
                            var pointStr = answer[1].Split(';').ToList();
                            var point = new Point(int.Parse(pointStr[0]), int.Parse(pointStr[1]));
                            var obj = ConvertObjectInEnum(answer[2]);
                            _process.StandardInput.WriteLine(_world.GetObject(point, obj).ToString());
                        }
                        if (answer[0] == "SleepTime")
                        {
                            _process.StandardInput.WriteLine(_world.FrogWantToSleep);
                        }
                        if (answer[0] == "MouthLength")
                        {
                            _process.StandardInput.WriteLine(_world.FrogMouthLength);
                        }
                        if (answer[0] == "Size")
                        {
                            _process.StandardInput.WriteLine(_world.Size.Width + " " + _world.Size.Height);
                        }
                        if (answer[0] == "Answer" && _ind == _world.GetTurn())
                        {
                            _world.SetDirection(DirectionFromString(answer[1]));
                            _world.SetTurn(0);
                            break;
                        }
                        if (_ind != _world.GetTurn())
                            break;
                    }
                }
            }
        }

        private Direction DirectionFromString(string direct)
        {
            switch (direct)
            {
                case "Down":
                    return Direction.Down;
                case "Up":
                    return Direction.Up;
                case "Left":
                    return Direction.Left;
                case "Right":
                    return Direction.Right;
                default:
                    return Direction.None;
            }
        }
    }

    public class World
    {
        public int FrogMouthLength { get; private set; }
        public int FrogWantToSleep { get; private set; }
        public int LifeTime { get; private set; }
        public long Time { get; set; }
        public Size Size { get; private set; }
        public int ObjectsCount { get { return Objects.Count; } }

        public HashSet<WorldObject> Objects = new HashSet<WorldObject>();
        public Dictionary<Point, HashSet<WorldObject>> Cells = new Dictionary<Point, HashSet<WorldObject>>();
        public int WhiteScore { get; set; }
        public int BlackScore { get; set; }

        public IAntAI WhiteAntAI;
        public IAntAI BlackAntAI;

        private Process process;
        private Thread BlackThread;
        private Thread WhiteThread;
        public static int Turn = 0;
        public static Point TemporaryLocation;
        public static Direction Direction = Direction.None;

        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }

        public Direction GetDirection()
        {
            return Direction;
        }

        public void SetTurn(int a)
        {
            Turn = a;
        }

        public int GetTurn()
        {
            return Turn;
        }

        public void SetLocation(Point point)
        {
            TemporaryLocation = point;
        }

        public Point GetLocation()
        {
            return TemporaryLocation;
        }

        public World(IList<string> args)
        {
            try
            {
                FrogMouthLength = int.Parse(args[1]);
                FrogWantToSleep = int.Parse(args[2]);
                LifeTime = int.Parse(args[0]);
                var location = Application.ExecutablePath;
                for (var i = 0; i < 3; i++)
                    location = location.Substring(0, location.LastIndexOf('\\'));
                var botsHome = location.Substring(0, location.LastIndexOf('\\')) + "\\BotsHome\\";
                location += "\\Players\\";
                var blackInitialisation = new BotInitialisation(location + args[4], botsHome + "BlackBot.exe", -1, this);
                BlackThread = new Thread(blackInitialisation.Start);
                BlackThread.IsBackground = true;
                BlackThread.Start();

                var whiteInitialisation = new BotInitialisation(location + args[3], botsHome + "WhiteBot.exe", 1, this);
                WhiteThread = new Thread(whiteInitialisation.Start);
                WhiteThread.IsBackground = true;
                WhiteThread.Start();

                //var aiImplementationWhite =
                //    Assembly.UnsafeLoadFrom(location + args[3])
                //        .GetTypes()
                //        .First(type => type.GetInterfaces().Any(i => i == typeof(IAntAI)));
                //var aiImplementationBlack =
                //    Assembly.UnsafeLoadFrom(location + args[4])
                //        .GetTypes()
                //        .First(type => type.GetInterfaces().Any(i => i == typeof(IAntAI)));
                //WhiteAntAI = (IAntAI) Activator.CreateInstance(aiImplementationWhite);
                //BlackAntAI = (IAntAI) Activator.CreateInstance(aiImplementationBlack);
            }
            catch (Exception e)
            {
                File.AppendAllText("WorldCreateExc.txt", e.ToString());
            }
        }

        

        public void MakeStep()
        {
            try
            {
                if (LifeTime <= 0) return;
                foreach (var obj in Objects.ToList().Where(Objects.Contains))
                {
                    RemoveObject(obj);
                    obj.Location = obj.Destination;
                    AddObject(obj);
                    obj.Act(this);
                }
                Time++;
                LifeTime--;
                GenerateNewFood();
            }
            catch (Exception e)
            {
                File.AppendAllText("WorldStepExc.txt", e.ToString());
            }
        }

        private void GenerateNewFood()
        {
            if (ObjectsCount > Size.Width * Size.Height / 2 || Time % 10 != 0)
                return;
            var random = new Random();
            while (true)
            {
                var x = random.Next(Size.Width);
                var y = random.Next(Size.Height);
                if (x < 0 || x >= Size.Width || y < 0 || y >= Size.Height)
                    continue;
                if (GetObject(new Point(x, y), AntColour.None) != Object.None ||
                    GetObject(new Point(Size.Width - x, Size.Height - y), AntColour.None) != Object.None) continue;
                AddObject(new Food(new Point(x, y)));
                AddObject(new Food(new Point(Size.Width - x, Size.Height - y)));
                break;
            }
        }

        public Object GetObject(Point location, AntColour colour)
        {
            if (!Cells.ContainsKey(location))
                return Object.None;
            foreach (var obj in Cells[location])
            {
                if (colour == AntColour.None)
                    return obj.GetObjectType();
                if (obj.GetColourOrNone() == AntColour.None)
                    return obj.GetObjectType();
                return obj.GetColourOrNone() == colour ? Object.YourAnt : Object.EnemyAnt;
            }
            return Object.None;
        }

        public void AddObject(WorldObject obj)
        {
            Objects.Add(obj);
            if (!Cells.ContainsKey(obj.Location)) Cells[obj.Location] = new HashSet<WorldObject>();
            Cells[obj.Location].Add(obj);
        }

        public void RemoveObject(WorldObject obj)
        {
            Objects.Remove(obj);
            Cells.Remove(obj.Location);
        }

        public void FreezeWorldSize()
        {
            Size = Objects.Any() ? new Size(Objects.Max(o => o.Location.X) + 1, Objects.Max(o => o.Location.Y) + 1) : new Size(1, 1);
        }

        public void CloseAllProcess()
        {
            BlackThread.Abort();
            WhiteThread.Abort();
        }
    }
}
