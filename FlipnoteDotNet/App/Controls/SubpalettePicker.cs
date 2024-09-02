using FlipnoteDotNet.Core.Utils;
using FlipnoteDotNet.Drawing;
using FlipnoteDotNet.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private int[] fColorIndices = null;
        public int[] ColorIndices
        {
            get => fColorIndices;
            set
            {
                fColorIndices = value;
                Invalidate();
            }
        }

        private Font IndexFont = new Font(FontFamily.GenericSansSerif, 6);

        protected override void OnPaint(PaintEventArgs e)
        {
            ComputeColorButtonsSize(out var w, out var h);
            int x = 0, y = 0;
            for (int i = 0; i < Palette.Colors.Length; i++)
            {
                DrawColor(e.Graphics, Palette.Colors[i], x, y, w, h, i==FirstIndex);
                x += w;
                if (x + w > Width)
                {
                    x = 0;
                    y += h;
                }
            }
            if (fColorIndices != null)
            {
                int colorsPerRow = Width / w;
                for (int i = 0; i < fColorIndices.Length; i++)
                {
                    bool sel = fColorIndices[i] == FirstIndex;
                    int ci = fColorIndices[i];
                    DrawFrame(e.Graphics, IndexFont, i, (ci % colorsPerRow) * w, (ci / colorsPerRow) * h, w, h, sel);
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
                g.DrawRectangle(Pens.DodgerBlue, x, y, w, h);
            }
        }

        private static void DrawFrame(Graphics g, Font font, int index, int x, int y, int w, int h, bool selected)
        {
            x += 2; y += 2;
            w -= 4; h -= 4;
            using (var pen = new Pen(Color.Black, 3)) 
                g.DrawRectangle(pen, x, y, w, h);
            var bpen = selected ? Pens.DodgerBlue : Pens.Wheat;
            var brush = selected ? Brushes.DodgerBlue : Brushes.Wheat;
            g.DrawRectangle(bpen, x, y, w, h);
            g.FillRectangle(brush, x, y, 10, 10);
            g.DrawString(index.ToString(), font, Brushes.Black, x, y, StringFormat.GenericTypographic);

        }

        protected override void OnResize(EventArgs e)
        {
            Invalidate();
        }

        enum ActionState { ChooseFirst, ChooseSecond }

        ActionState State = ActionState.ChooseFirst;
        int FirstIndex = -1;
        int SecondIndex = -1;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ComputeColorButtonsSize(out var w, out var h);
            int x = e.X.Clamp(0, Width - 1) / w;
            int y = e.Y.Clamp(0, Height - 1) / h;
            int colorsPerRow = Width / w;
            if (x >= colorsPerRow) return;
            int selIndex = y * colorsPerRow + x;
            if (selIndex >= Palette.Colors.Length) return;

            Debug.WriteLine($"MsDown {State}");

            switch(State)
            {
                case ActionState.ChooseFirst:
                    {
                        Debug.WriteLine("Here?");
                        FirstIndex = selIndex;
                        State = ActionState.ChooseSecond;
                        Invalidate();
                        break;
                    }
                case ActionState.ChooseSecond:
                    {                        
                        SecondIndex = selIndex;
                        Debug.WriteLine($"Here2? {FirstIndex}, {SecondIndex}");
                        SwapColors();
                        FirstIndex = SecondIndex = -1;
                        State = ActionState.ChooseFirst;
                        Invalidate();
                        break;
                    }
            }
        }

        private void SwapColors()
        {
            if (FirstIndex == SecondIndex) return;
            int fi = fColorIndices != null ? Array.IndexOf(fColorIndices, FirstIndex) : -1;
            int si = fColorIndices != null ? Array.IndexOf(fColorIndices, SecondIndex) : -1;

            Debug.WriteLine($"fi,si = {(fi, si)}");

            if(fi>=0 && si>=0)
            {
                var indices = fColorIndices.CloneArray();
                (indices[fi], indices[si]) = (indices[si], indices[fi]);
                fColorIndices = indices;
                ColorIndicesChanged?.Invoke(this, indices.CloneArray());
            }
            else if(fi>=0)
            {
                var indices = fColorIndices.CloneArray();
                indices[fi] = SecondIndex;
                fColorIndices = indices;
                ColorIndicesChanged?.Invoke(this, indices.CloneArray());
            }
            else if(si>=0)
            {
                var indices = fColorIndices.CloneArray();
                indices[si] = FirstIndex;
                fColorIndices = indices;
                ColorIndicesChanged?.Invoke(this, indices.CloneArray());
            }            
        }

        public event EventHandler<int[]> ColorIndicesChanged;

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
    }
}
