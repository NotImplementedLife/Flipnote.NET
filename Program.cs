using System;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    internal static class Program
    {        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Constants.Init();

            Application.Run(new MainForm());
        }
    }
}