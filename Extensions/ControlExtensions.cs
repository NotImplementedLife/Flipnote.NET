using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.Extensions
{
    internal static class ControlExtensions
    {
        public static void EnableDoubleBuffer(this Control control)
        {
            typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(control, true);
        }

        public static void DisableSelectable(this Control control)
        {
            typeof(Control).GetMethod("SetStyle", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(control, new object[] { ControlStyles.Selectable, false });
        }


        
    }
}
