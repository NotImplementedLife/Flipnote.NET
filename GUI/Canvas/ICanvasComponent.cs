using FlipnoteDotNet.GUI.Canvas.Drawing;
using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas
{
    public interface ICanvasComponent
    {
        Rectangle Bounds { get; set; }
        Point Location { get; set; }
        Size Size { get; set; }

        void OnPaint(CanvasGraphics g);

    }
}
