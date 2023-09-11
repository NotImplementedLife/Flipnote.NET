using FlipnoteDotNet.GUI.Canvas.Drawing;
using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Components
{
    public class CanvasComponent : ICanvasComponent
    {
        private Rectangle _Bounds;

        public Rectangle Bounds
        {
            get => _Bounds;
            set => _Bounds = value;
        }

        public Point Location
        {
            get => _Bounds.Location;
            set => _Bounds.Location = value;
        }
        public Size Size
        {
            get => _Bounds.Size;
            set => _Bounds.Size = value;
        }
        public bool IsFixed { get; set; } = false;

        public virtual void OnPaint(CanvasGraphics g)
        {
            g.DrawRectangle(new Pen(Brushes.Gray, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, Bounds);
        }
    }
}
