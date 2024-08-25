using FlipnoteDotNet.App.Controls;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service;
using FlipnoteDotNet.PropertyEditor.Editors;
using SuperContextMenu;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FlipnoteDotNet.App.Editors
{
    internal class FramePaperColorIndexEditor : Editor<int>
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
                    fFrameConfig.PaperColorChanged += FFrameConfig_PaperColorChanged;
                    fFrameConfig.ColorIndicesChanged += FFrameConfig_ColorIndicesChanged;
                    PopedContextMenu.Palette = fFrameConfig.ActualPalette;
                    fValue = PopedContextMenu.SelectedIndex = fFrameConfig.PaperColorIndex;
                    return;
                }
                throw new InvalidOperationException("FrameConfig already set");
            }
        }

        private void FFrameConfig_ColorIndicesChanged(object sender, EventArgs e)
        {            
            PopedContextMenu.SelectedIndex = fFrameConfig.PaperColorIndex;
            Invalidate();
        }

        private void FFrameConfig_PaperColorChanged(object sender, EventArgs e)
        {
            PopedContextMenu.Palette = fFrameConfig.ActualPalette;
            Invalidate();
        }

        public FramePaperColorIndexEditor() : base(invalidateAfterValueChanged: true)
        {
            PoperContainer = new PoperContainer(PopedContextMenu);
            PopedContextMenu.ColorChanged += PopedContextMenu_ColorChanged;            
        }

        private void PopedContextMenu_ColorChanged(object sender, EventArgs e)
        {
            var oldValue = fValue;
            Value = PopedContextMenu.SelectedIndex;
            TriggerUserValueChanged(oldValue, fValue, preview: false);
        }

        private int Width, Height;
        private int X, Y;

        public override void OnPaint(Graphics g, Font font)
        {
            Width = (int)g.ClipBounds.Width;
            Height = (int)g.ClipBounds.Height;
            X = (int)g.ClipBounds.X;
            Y = (int)g.ClipBounds.Y;
            FrameConfig = (Target as FrameProxy).Frame.FrameConfig;       
            var rect = Rectangle.Truncate(g.ClipBounds);
            ButtonRenderer.DrawButton(g, rect, PushButtonState.Normal);
            using (var b = new SolidBrush(FrameConfig.PaperColor))
                g.FillRectangle(b, rect.Left + 4, rect.Top + 4, rect.Width - 8, rect.Height - 8);
        }

        public override void OnMouseDown(MouseButtons buttons, int x, int y)
        {
            PopedContextMenu.Size = new Size(Width, 50);
            PopedContextMenu.SelectedIndex = fValue;
            //Debug.WriteLine($"");
            PoperContainer.Show(Parent, new Rectangle(Parent.Width - Width, 0, Width, Y + Height));
        }

        private readonly PaletteColorChooser PopedContextMenu = new PaletteColorChooser();
        private readonly PoperContainer PoperContainer;


    }
}