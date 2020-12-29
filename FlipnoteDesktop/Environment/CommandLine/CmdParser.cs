using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlipnoteDesktop.Environment.CommandLine
{
    internal static class CmdParser
    {
        public static void Parse(string[] args)
        {
            if (args.Length == 0)
            {                
                return;
            }
            if (args[0].Length > 4 && args[0].Substring(args[0].Length - 4) == ".ppm" && File.Exists(args[0]))
            {                
                Console.WriteLine("File detected");
                FileName = args[0];                
            }
            else if(args[0]=="-h" || args[0]=="--help" || args[0]=="help")
            {
                Console.WriteLine("Help message here");
                LaunchApp = false;
            }
            else if(args[0]=="--version" || args[0]=="version")
            {
                Console.WriteLine($"FlipnoteDesktop {App.Version}");
                LaunchApp = false;
            }
        }

        public static string FileName = null;
        public static bool OpenEdit = false;
        public static bool LaunchApp = true;
    }
}
