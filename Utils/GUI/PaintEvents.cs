using FlipnoteDotNet.Extensions;
using System.Drawing;
using System.Windows.Forms;
using static FlipnoteDotNet.Constants;

namespace FlipnoteDotNet.Utils.GUI
{
    internal static class PaintEvents
    {
        public static void BottomLine_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            e.Graphics.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(), 0, control.Height - 2, control.Width, control.Height - 2);
        }

        public static void TopLine_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            e.Graphics.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(), 0, 2, control.Width, 2);
        }


        public static void BackgroundControlPaint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            var pos = GetLocationRelativeToForm(control);
            var dx = pos.X % 16;
            var dy = pos.Y % 16;
            e.Graphics.FillRectangle(Constants.Brushes.GetWindowBackgroundBrush(dx, dy), new Rectangle(Point.Empty, control.Size));
        }

        private static Point GetLocationRelativeToForm(Control c)
        {
            if (c is Form) return Point.Empty;
            if (c.Parent is Form) return c.Location;
            var result = c.Location;
            result.Offset(GetLocationRelativeToForm(c.Parent));
            return result;
        }
    }
}
