using FlipnoteDotNet.Core.Utils;
using FlipnoteDotNet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Controls
{
    internal class SubpalettePicker : Control
    {
        public SubpalettePicker()
        {
            DoubleBuffered = true;
        }

        private Palette fPalette = Palettes.FlipnotePalette;        
        public Palette Palette
        {
            get => fPalette;
            set
            {
                fPalette = value;
                Invalidate();
            }
        }        
        protected override void OnPaint(PaintEventArgs e)
        {
            ComputeColorButtonsSize(out var w, out var h);
            int x = 0, y = 0;
            for (int i = 0; i < Palette.Colors.Length; i++)
            {
                DrawColor(e.Graphics, Palette.Colors[i], x, y, w, h, false);
                x += w;
                if (x + w > Width)
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
            if (x >= colorsPerRow) return;
            int newIndex = y * colorsPerRow + x;
            if (newIndex >= Palette.Colors.Length) return;
            //if (SelectedIndex == newIndex) return;
            //SelectedIndex = newIndex;
            //ColorChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ComputeColorButtonsSize(out int width, out int height)
        {
            height = Height;
            width = Width / Palette.Colors.Length;
            if (width < 16)
            {
                height = Height / 2;
                width = 2 * Width / Palette.Colors.Length;
            }
            if (width < 16)
            {
                height = Height / 4;
                width = 4 * Width / Palette.Colors.Length;
            }
        }

        public event EventHandler ColorChanged;
    }
}
