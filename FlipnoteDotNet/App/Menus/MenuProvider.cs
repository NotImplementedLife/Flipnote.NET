using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Menus
{
    public class MenuProvider
    {
        public static F PrepareForm<F>(F form, Action<F, ToolStripItem[]> addMenu, string menuName = "Menu") where F:Form
        {
            var items = MenuStripLoader.Load(typeof(F), menuName);
            addMenu(form, items.ToArray());
            return form;
        }


    }
}
