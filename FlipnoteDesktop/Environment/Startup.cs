using FlipnoteDesktop.Environment.CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlipnoteDesktop.Environment
{
    public static class Startup
    {
        [STAThread]
        public static int Main(string[] args)
        {
            var handle = GetStdHandle(-11);
            AttachConsole(-1);
            
            CmdParser.Parse(args);
            if(CmdParser.LaunchApp)
            {
                var App = new App();
                App.InitializeComponent();
                App.Run();                
            }
            return 0;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);
    }
}
