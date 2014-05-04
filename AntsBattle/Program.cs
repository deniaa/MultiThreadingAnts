using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AntsBattle
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var data = new StartWindowData();
                Application.Run(new StartWindow(data));
            }
            catch (Exception e)
            {
                File.AppendAllText("exceptions.txt", e.ToString());
            }
        }
    }
}
