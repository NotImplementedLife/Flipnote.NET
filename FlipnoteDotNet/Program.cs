using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.GUI.Forms;
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
            InitializerService.Register(typeof(PPMLib.Initializer), typeof(BinarySerializer));
            InitializerService.Run();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
