using FlipnoteDotNet.GUI.Canvas.Drawing;
using System.Diagnostics;
using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Components
{
    public class SimpleRectangle : CanvasComponent
    {
        public Pen Pen { get; set; } = Pens.Black;

        public SimpleRectangle(Rectangle rectangle)
        {
            Bounds = rectangle;
        }


        public SimpleRectangle(Point position, Size size)
        {
            Location = position;
            Size = size;
        }

        public override void OnPaint(CanvasGraphics g)
        {
            g.DrawRectangle(Pen, Bounds);
        }
    }
}
