using FlipnoteDotNet.Core.Utils;
using FlipnoteDotNet.Drawing;
using System.Runtime.InteropServices;

namespace FlipnoteDotNet.App.Controls
{
    public class PaletteColorChooser : Control
    {
        public PaletteColorChooser()
        {            
            DoubleBuffered = true;
        }

        private Palette fPalette = Palettes.FlipnotePalette;
        private int fSelectedIndex = 0;
        public Palette Palette
        {
            get => fPalette;
            set
            {
                fPalette = value;
                Invalidate();
            }
        }

        public int SelectedIndex
        {
            get => fSelectedIndex;
            set
            {
                if (fSelectedIndex == value) return;
                fSelectedIndex = value;
                ColorChanged?.Invoke(this, EventArgs.Empty);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ComputeColorButtonsSize(out var w, out var h);
            int x = 0, y = 0;
            for(int i=0;i<Palette.Colors.Length;i++)
            {
                DrawColor(e.Graphics, Palette.Colors[i], x, y, w, h, i == fSelectedIndex);
                x+= w;
                if (x >= Width) 
                {
                    x = 0;
                    y += h;
                }
            }
        }

        private static void DrawColor(Graphics g, Color color, int x, int y, int w, int h, bool selected)
        {
            x += 2; y += 2;
            w -= 4; h -= 4;
            using (var brush = new SolidBrush(color))            
                g.FillRectangle(brush, x, y, w, h);
            if (!selected)
            {
                g.DrawRectangle(Pens.Black, x, y, w, h);
            }
            else
            {
                using (var pen = new Pen(Color.Black, 3))
                    g.DrawRectangle(pen, x, y, w, h);
                g.DrawRectangle(Pens.Wheat, x, y, w, h);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ComputeColorButtonsSize(out var w, out var h);
            int x = e.X.Clamp(0, Width - 1) / w;
            int y = e.Y.Clamp(0, Height - 1) / h;
            int colorsPerRow = Width / w;
            SelectedIndex = y * colorsPerRow + x;
        }

        private void ComputeColorButtonsSize(out int width, out int height)
        {
            height = Height;
            width = Width / Palette.Colors.Length;
        }

        public event EventHandler ColorChanged;

    }
}
