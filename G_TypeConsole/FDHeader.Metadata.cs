using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FDHeader
{
    public static class Metadata
    {
        public static string PluginName = "TypeConsole";
        public static List<string> Exports = new List<string>
        {
            "G_TypeConsole.TypeConsoleGenerator"
        };
    }
}
