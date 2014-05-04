using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntsBattle
{
    public class StartWindowData
    {
        public List<string> Players;
        public List<string> Maps;
        public string[] Args;

        public StartWindowData()
        {
            var location = Application.ExecutablePath;
            for (var i = 0; i < 3; i++)
            {
                location = location.Substring(0, location.LastIndexOf('\\'));
            }
            location += "\\Players";
            var info = new DirectoryInfo(location);
            var files = info.GetFiles();
            Players = new List<string>();
            foreach (var file in files.Where(x => x.Extension == ".dll").Select(x => x.ToString()))
                Players.Add(file.Substring(0, file.LastIndexOf('.')));

            var mapsInfo = new DirectoryInfo(@"Maps");
            var maps = mapsInfo.GetFiles();
            Maps = new List<string>();
            foreach (var map in maps.Where(x => x.Extension == ".txt").Select(x => x.ToString()))
                Maps.Add(map.Substring(0, map.LastIndexOf('.')));
        }
    }
}
