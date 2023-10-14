using FlipnoteDotNet.GUI.Canvas.Drawing;
using System;
using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Components
{
    internal class BitmapComponent : CanvasComponent
    {        
        public Bitmap Bitmap { get; }
        public override bool IsResizeable { get; set; } = true;

        public BitmapComponent(Bitmap bitmap) : this(bitmap, Point.Empty) { }        

        public BitmapComponent(Bitmap bitmap, Point location)
        {
            Bitmap = bitmap;
            Location = location;
            Size = bitmap.Size;
        }

        public BitmapComponent(Bitmap bitmap, Rectangle rectangle)
        {
            Bitmap = bitmap;
            Location = rectangle.Location;
            Size = rectangle.Size;
        }

        public BitmapComponent(Bitmap bitmap, int x, int y) : this(bitmap, new Point(x, y)) { }
        public BitmapComponent(Bitmap bitmap, int x, int y, int width, int height) : this(bitmap, new Rectangle(x, y, width, height)) { }

        public override void OnPaint(CanvasGraphics g)
        {
            g.DrawImage(Bitmap, Location, Size);
            base.OnPaint(g);
        }
    }
}
