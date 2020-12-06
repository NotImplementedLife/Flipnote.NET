using FlipnoteDesktop.Environment.Canvas;
using FlipnoteDesktop.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlipnoteDesktop.External.Plugins
{
    internal static class PluginManager
    {        
        public static void ImportPlugins(string dllpath)
        {
            try
            {                
                Assembly assembly = Assembly.LoadFrom(dllpath);
                Type type = assembly.GetType("FDHeader.Metadata");
                var asm_name = type.GetField("PluginName").GetValue(null) as string;
                var exports = type.GetField("Exports").GetValue(null) as List<string>;
                for (int i = 0, cnt = exports.Count; i < cnt; i++)
                {
                    try
                    {
                        var _class = assembly.GetType(exports[i]);
                        if(_class.IsSubclassOf(typeof(Generator)))
                        {
                            Plugins.Add(new PluginData(asm_name, exports[i], PluginTypes.Generator, _class));                            
                        }
                    }
                    catch(TypeLoadException)
                    {
                        Plugins.Add(PluginData.Failed(asm_name, exports[i]));
                    }
                }                                
            }
            catch(Exception)
            {                
            }
        }

        public static List<PluginData> Plugins = new List<PluginData>();

        public static void Populate(MainWindow wnd)
        {
            for (int i = 0, cnt = Plugins.Count; i < cnt; i++)
            {
                if(Plugins[i].PluginType==PluginTypes.Generator)
                {
                    var item = new MenuItem();
                    item.Header = Plugins[i].PluginName;
                    item.Tag = Plugins[i].Class;
                    item.Click += wnd.GeneratorMenuItem_Click;
                    wnd.GeneratorsMenuItem.Items.Add(item);
                }
            }
        }
    }
}
