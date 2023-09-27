using FlipnoteDotNet.Attributes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Menu
{
    internal static class MenuItemsLoader
    {
        public static AttributesManager<MenuItemAttribute, ToolStripMenuItem> MenuItemTypes = new AttributesManager<MenuItemAttribute, ToolStripMenuItem>();

        public static void LoadMenuItems(MenuStrip menu)
        {
            var form = menu.FindForm();
            var formType = form.GetType();

            foreach (var record in MenuItemTypes.Where(r => r.Attribute.TargetFormType == formType))
            {
                var path = record.Attribute.MenuPath.Split('/');

                if (path.Length == 1)
                {
                    var item = Activator.CreateInstance(record.Type, form) as ToolStripMenuItem;
                    item.Text = item.Name = path[0];
                    menu.Items.Add(item);
                    continue;
                }
                if (path.Length > 1)
                {
                    var parent = GetOrCreateMenuItem(menu, path.Take(path.Length - 1).ToArray());
                    var child = Activator.CreateInstance(record.Type, form) as ToolStripMenuItem;
                    child.Text = child.Name = path.Last();
                    parent.DropDown.Items.Add(child);
                    continue;
                }
            }
        }

        private static ToolStripMenuItem GetOrCreateMenuItem(MenuStrip menu, string[] path)
        {
            if (!menu.Items.ContainsKey(path[0]))
                menu.Items.Add(new ToolStripMenuItem(path[0]) { Name = path[0] });

            var item = menu.Items[path[0]] as ToolStripMenuItem;

            for (int i = 1; i < path.Length; i++)
            {
                if (!item.HasDropDown || !item.DropDown.Items.ContainsKey(path[i]))
                {
                    item.DropDown.Items.Add(new ToolStripMenuItem(path[i]) { Name = path[i] });
                }
                item = item.DropDown.Items[path[i]] as ToolStripMenuItem;
            }
            return item;
        }
    }
}
