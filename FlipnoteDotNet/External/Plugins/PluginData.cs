using FlipnoteDotNet.Environment.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.External.Plugins
{
    internal class PluginData
    {
        public PluginData(string asmname, string classname, PluginTypes type, Type _class)
        {
            AssemblyName = asmname;
            ClassName = classname;
            PluginType = type;            
            Class = _class;
            if (PluginType == PluginTypes.Generator) 
            {
                PluginName = (Activator.CreateInstance(Class) as Generator).Name;
            }
        }

        public static PluginData Failed(string asmname, string pluginname)
            => new PluginData(asmname, pluginname, PluginTypes.Failed, null);                

        public string AssemblyName { get; }
        public string ClassName { get; }
        public string PluginName { get; }
        public PluginTypes PluginType { get; }
        public Type Class { get; }
    }
}
