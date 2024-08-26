using FlipnoteDotNet.App.Controls;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service;
using FlipnoteDotNet.PropertyEditor.Editors;
using SuperContextMenu;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace FlipnoteDotNet.App.Editors
{
    internal class FrameSubpaletteEditor : Editor<int[]>
    {
        public override int RequestedTextRows => 1;

        private FrameConfig fFrameConfig;
        public FrameConfig FrameConfig
        {
            get => fFrameConfig;
            set
            {
                if (fFrameConfig == value) return;
                if (fFrameConfig == null)
                {
                    fFrameConfig = value;
                    fFrameConfig.ColorIndicesChanged += FFrameConfig_ColorIndicesChanged;
                    PopedContextMenu.Palette = fFrameConfig.Palette;

                    //PopedContextMenu.Palette = fFrameConfig.ActualPalette;
                    //fValue = PopedContextMenu.SelectedIndex = fFrameConfig.PaperColorIndex;
                    return;
                }
                throw new InvalidOperationException("FrameConfig already set");
            }
        }

        private void FFrameConfig_ColorIndicesChanged(object sender, EventArgs e)
        {            
            Invalidate();
        }

        public FrameSubpaletteEditor() : base(invalidateAfterValueChanged: true) 
        {
            PoperContainer = new PoperContainer(PopedContextMenu);
            //PopedContextMenu.ColorChanged += PopedContextMenu_ColorChanged;
        }

        int X, Y, Width, Height;

        public override void OnPaint(Graphics g, Font font)
        {            
            FrameConfig = (Target as FrameProxy).Frame.FrameConfig;
            
            if (fValue == null)
            {
                g.DrawString("(none)", font, Brushes.Black, g.ClipBounds, StringFormat.GenericTypographic);
            }
            else
            {
                Width = (int)g.ClipBounds.Width;
                Height = (int)g.ClipBounds.Height;
                X = (int)g.ClipBounds.X;
                Y = (int)g.ClipBounds.Y;
                var rect = new Rectangle(X, Y, Width, Height);
                ButtonRenderer.DrawButton(g, rect, PushButtonState.Normal);

                X += 4;
                Y += 4;
                Width -= 8;
                Height -= 8;

                int w = (int)(Width / fFrameConfig.ActualPalette.Colors.Length);
                for (int i = 0; i < fFrameConfig.ActualPalette.Colors.Length; i++)
                {
                    using(var b=new SolidBrush(fFrameConfig.ActualPalette.Colors[i]))
                    {
                        g.FillRectangle(b, X + i * w, Y, w, Height);
                    }
                }
            }
        }

        public override void OnMouseDown(MouseButtons buttons, int x, int y)
        {
            PopedContextMenu.Size = new Size(Width, 50);
            PoperContainer.Show(Parent, new Rectangle(Parent.Width - Width, 0, Width, Y + Height));
            //PoperContainer
        }

        private readonly SubpalettePicker PopedContextMenu = new SubpalettePicker();
        private readonly PoperContainer PoperContainer;
    }
}
