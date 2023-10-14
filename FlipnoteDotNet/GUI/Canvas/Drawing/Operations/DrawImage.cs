using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Drawing.Operations
{
    internal class DrawImage : ICanvasGraphicsOperation
    {
        public Bitmap Bitmap { get; }
        public Point Point { get; }
        public Size Size { get; }
        public DrawImage(Bitmap bitmap, Point point)
        {
            Bitmap = bitmap;
            Point = point;
            Size = bitmap.Size;
        }

        public DrawImage(Bitmap bitmap, Point point, Size size)
        {
            Bitmap = bitmap;
            Point = point;            
            Size = size;
        }

        public void Execute(CanvasGraphicsRenderer renderer) => renderer.DrawImage(Bitmap, Point, Size);
    }
}
