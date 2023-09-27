using FlipnoteDotNet.Utils.Manipulator;
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
            Manipulators.Initialize();            

            Application.Run(new MainForm());
        }
    }
}