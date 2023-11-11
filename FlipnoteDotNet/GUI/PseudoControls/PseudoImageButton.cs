using System.Drawing;
using System;
using PPMLib.Winforms;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.PseudoControls
{
    public class PseudoImageButton : PseudoControl
    {
        private readonly Image pImage;

        public PseudoImageButton(Image image)
        {
            pImage = image;
        }        

        private bool pIsChecked = false;
        public bool IsChecked
        {
            get => pIsChecked;
            set
            {
                pIsChecked = value;
                Invalidate();
            }
        }

        private bool pIsCheckable = false;
        public bool IsCheckable
        {
            get => pIsCheckable;
            set
            {
                pIsCheckable = value;
                IsChecked = false;
            }
        }

        protected override void OnPaint(Graphics g, Rectangle bounds, bool isHovered)
        {
            if (pIsChecked)
                g.FillRectangle(FlipnoteBrushes.ThemePrimary, bounds);

            if (isHovered && IsEnabled) 
                using (var brush = new SolidBrush(Color.FromArgb(64, FlipnoteColors.ThemeAccent)))
                    g.FillRectangle(brush, bounds);
            float scale = 1;
            if (pImage.Width > bounds.Width || pImage.Height > bounds.Height)
                scale = Math.Min(1.0f * bounds.Width / pImage.Width, 1.0f * bounds.Height / pImage.Height);

            var w = pImage.Width * scale;
            var h = pImage.Height * scale;
            var x = bounds.Left + (bounds.Width - w) / 2;
            var y = bounds.Top + (bounds.Height - h) / 2;

            if (IsEnabled) 
                g.DrawImage(pImage, x, y, w, h);
            else
                g.DrawImage(ToolStripRenderer.CreateDisabledImage(pImage), x, y, w, h);            
        }        
    }
}
