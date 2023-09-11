using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Drawing.Operations
{
    internal class DrawRectangle : ICanvasGraphicsOperation
    {
        public Pen Pen { get; }
        public Rectangle Rectangle { get; }

        public DrawRectangle(Pen pen, Rectangle rectangle)
        {
            Pen = pen;
            Rectangle = rectangle;
        }

        public void Execute(CanvasGraphicsRenderer renderer) => renderer.DrawRectangle(Pen, Rectangle);
    }
}
