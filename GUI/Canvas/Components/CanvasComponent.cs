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

        public virtual void OnPaint(CanvasGraphics g) { }        
    }
}
