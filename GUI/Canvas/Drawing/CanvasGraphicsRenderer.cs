using System.Diagnostics;
using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Drawing
{
    internal class CanvasGraphicsRenderer
    {
        public CanvasGraphicsRenderer(Graphics graphics, Point scale, Point offset)
        {
            Graphics = graphics;
            Scale = scale;
            Offset = offset;
        }

        public CanvasGraphicsRenderer(Graphics graphics) : this(graphics, new Point(100, 100), Point.Empty) { }

        public Graphics Graphics { get; }
        public Point Scale { get; }
        public Point Offset { get; }

        public void DrawLine(Pen pen, Point p1, Point p2) 
            => Graphics.DrawLine(pen, TransformToGraphics(p1), TransformToGraphics(p2));

        public void DrawRectangle(Pen pen, Rectangle rectangle)
        {            
            Graphics.DrawRectangle(pen, TransformToGraphics(rectangle));
        }

        public void FillRectangle(Brush brush, Rectangle rectangle)
            => Graphics.FillRectangle(brush, TransformToGraphics(rectangle));



        private Point TransformToGraphics(Point p)
        {
            var x = Offset.X + p.X * Scale.X / 100;
            var y = Offset.Y + p.Y * Scale.Y / 100;
            return new Point(x, y);
        }

        private Rectangle TransformToGraphics(Rectangle rectangle)
        {
            var topLeft = TransformToGraphics(new Point(rectangle.Left, rectangle.Top));
            var bottomRight = TransformToGraphics(new Point(rectangle.Right, rectangle.Bottom));
            var size = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
            return new Rectangle(topLeft, size);
        }


    }
}
