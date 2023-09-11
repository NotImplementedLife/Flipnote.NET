using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Drawing.Operations
{
    internal class FillRectangle : ICanvasGraphicsOperation
    {
        public Brush Brush { get; }
        public Rectangle Rectangle { get; }

        public FillRectangle(Brush brush, Rectangle rectangle)
        {
            Brush = brush;
            Rectangle = rectangle;
        }

        public void Execute(CanvasGraphicsRenderer renderer) => renderer.FillRectangle(Brush, Rectangle);
    }
}
