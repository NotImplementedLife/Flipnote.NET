using System.Diagnostics;
using System.Reflection;

namespace FlipnoteDotNet.App.Menus
{
    public static class MenuStripLoader
    {
        private const char Delim = 'Ⱝ';

        public static List<ToolStripMenuItem> Load(Type type, string menuName="Menu")
        {            
            menuName += Delim;
            var methods = from m in type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                          where m.Name.StartsWith(menuName)
                          select m;
            var items = new List<ToolStripMenuItem>();

            foreach(var method in methods)
            {
                var path = SplitPath(method.Name);
                Debug.WriteLine(string.Join("->", path));
                AddItem(items, path, method);                
            }

            return items;
        }

        private static void AddItem(List<ToolStripMenuItem> items, string[] path, MethodInfo method)
        {
            var item = items.Find(it => it.Text == path[0]);
            if(item==null)
            {
                item = new ToolStripMenuItem(path[0]) { Name = path[0] };
                items.Add(item);
            }            
            if(path.Length==1)
            {
                item.Click += (o, e) => method.Invoke(o, null);
            }
            else
            {
                AddSubItem(item, path, 1, method);
            }
        }

        private static void AddSubItem(ToolStripMenuItem item, string[] path, int index, MethodInfo method)
        {
            var it = item.DropDownItems.Find(path[index], false).FirstOrDefault() as ToolStripMenuItem;
            if(it==null)
            {
                it = new ToolStripMenuItem(path[index]) { Name = path[index] };
                item.DropDownItems.Add(it);
            }

            if (index == path.Length - 1) 
            {
                it.Click += (o, e) => method.Invoke(o, null);
            }
            else
            {
                AddSubItem(it, path, index + 1, method);
            }           
        }

        private static string[] SplitPath(string path)
        {
            return path.Split(Delim).Skip(1).Select(c => c.Replace('_', ' ')).ToArray();
        }

    }
}
