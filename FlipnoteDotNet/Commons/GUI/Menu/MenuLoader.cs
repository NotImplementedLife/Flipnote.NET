using FlipnoteDotNet.Commons.Reflection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.Commons.GUI.Menu
{
    public static class MenuLoader
    {
        private static ToolStripItem FindOrCreateMenuItem(MenuStrip menu, string path)
        {
            var parts = path.Split('/');
            ToolStripMenuItem item = null;
            for(int i=0;i<parts.Length;i++)
            {
                if(item==null)
                {
                    if (menu.Items.ContainsKey(parts[i]))
                        item = menu.Items[parts[i]] as ToolStripMenuItem;
                    else
                    {
                        item = new ToolStripMenuItem(parts[i]) { Name = parts[i] };
                        menu.Items.Add(item);
                    }                    
                }
                else
                {
                    if (item.DropDown.Items.ContainsKey(parts[i]))
                        item = item.DropDown.Items[parts[i]] as ToolStripMenuItem;
                    else
                    {
                        var child = new ToolStripMenuItem(parts[i]) { Name = parts[i] };
                        item.DropDown.Items.Add(child);
                        item = child;
                    }
                }
            }
            return item;
        }

        public static void Load(MenuStrip menu, object menuItemsProvider)
        {
            var argTypes = new[] { typeof(object), typeof(EventArgs) };
            var menuItemsProviderType = menuItemsProvider.GetType();

            var methods = ClassScanner.GetMethods(menuItemsProviderType, argTypes, typeof(void), nonPublic: true);

            foreach (var method in methods)
            {                
                var attributes = method.GetCustomAttributes(true).Where(a => a is MenuItemAttribute).Cast<MenuItemAttribute>();                                

                foreach(var attr in attributes)
                {
                    var path = attr.Path ?? method.Name.Replace('Ⰰ', '/').Replace('_', ' ');

                    var menuItem = FindOrCreateMenuItem(menu, path);                                        
                    var handler = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), menuItemsProvider, method);
                    menuItem.Click += handler;
                }
            }
        }

    }
}
