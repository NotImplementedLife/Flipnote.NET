using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.Commons.GUI
{
    public static class Utils
    {
        private static PropertyInfo DoubleBufferedProperty = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void EnableDoubleBuffer(this Control c)
        {
            DoubleBufferedProperty.SetValue(c, true);
        }
    }
}
