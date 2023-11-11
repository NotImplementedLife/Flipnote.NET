using System;
using System.Drawing;

namespace FlipnoteDotNet.GUI.PseudoControls
{
    public class PseudoControl
    {
        private int pLeft;
        private int pTop;
        private int pWidth;
        private int pHeight;

        public Rectangle Bounds
        {
            get => new Rectangle(pLeft, pTop, pWidth, pHeight);
            set
            {
                pLeft = value.Left;
                pTop = value.Top;
                pWidth = value.Width;
                pHeight = value.Height;
            }
        }        

        public Size Size
        {
            get => new Size(pWidth, pHeight);
            set
            {
                pWidth =value.Width;
                pHeight = value.Height;
            }
        }

        private bool pIsEnabled = true;
        public bool IsEnabled
        {
            get => pIsEnabled;
            set
            {
                pIsEnabled = value;
                Invalidate();
            }
        }

        public event EventHandler RedrawRequired;

        public void Invalidate()
        {
            RedrawRequired?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPaint(Graphics g, Rectangle bounds, bool isHovered)
        {
            g.FillRectangle(Brushes.Red, bounds);
        }

        public void Paint(Graphics g, Rectangle bounds, bool isHovered) => OnPaint(g, bounds, isHovered);

        public event EventHandler Click;

        public void PerformClick()
        {
            if (IsEnabled)
                Click?.Invoke(this, EventArgs.Empty);
        }
    }
}
