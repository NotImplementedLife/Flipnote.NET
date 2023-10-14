using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Canvas.Misc
{
    public class ResizePoint
    {
        public ICanvasComponent Target { get; }
        public ResizeDirection ResizeDirection { get; }

        private Rectangle StoredBounds { get; set; }

        public ResizePoint(ICanvasComponent target, ResizeDirection resizeDirection)
        {
            Target = target;
            ResizeDirection = resizeDirection;            
        }

        public void SnapBounds()
        {
            StoredBounds = Target.Bounds;
        }

        public void DoResize(int dx, int dy)
        { 
            var x = StoredBounds.X;
            var y = StoredBounds.Y;
            var w = StoredBounds.Width;
            var h = StoredBounds.Height;

            if ((ResizeDirection & ResizeDirection.Top) != 0)
            {
                y += dy;
                h -= dy;
            }
            if ((ResizeDirection & ResizeDirection.Bottom) != 0)
                h += dy;

            if ((ResizeDirection & ResizeDirection.Left) != 0)
            {
                x += dx;
                w -= dx;
            }
            if ((ResizeDirection & ResizeDirection.Right) != 0)           
                w += dx;

            if (w <= 0 || h <= 0) return;

            Target.Bounds = new Rectangle(x, y, w, h);
        }

        public Point ClientLocation
        {
            get
            {
                switch(ResizeDirection)
                {
                    case ResizeDirection.TopLeft: return new Point(0, 0);
                    case ResizeDirection.Top: return new Point(Target.Size.Width/2, 0);
                    case ResizeDirection.TopRight: return new Point(Target.Size.Width, 0);
                    case ResizeDirection.BottomLeft: return new Point(0, Target.Size.Height);
                    case ResizeDirection.Bottom: return new Point(Target.Size.Width / 2, Target.Size.Height);
                    case ResizeDirection.BottomRight: return new Point(Target.Size.Width, Target.Size.Height);
                    case ResizeDirection.Left: return new Point(0, Target.Size.Height / 2);
                    case ResizeDirection.Right: return new Point(Target.Size.Width, Target.Size.Height / 2);
                    default: throw new InvalidOperationException("Unknown resize direction");
                }
            }
        }

        public Point CanvasLocation
        {
            get
            {
                var loc = ClientLocation;
                loc.Offset(Target.Location);
                return loc;
            }
        }

        public Cursor Cursor
        {
            get
            {
                switch (ResizeDirection)
                {
                    case ResizeDirection.TopLeft: return Cursors.SizeNWSE;
                    case ResizeDirection.Top: return Cursors.SizeNS;
                    case ResizeDirection.TopRight: return Cursors.SizeNESW;
                    case ResizeDirection.BottomLeft: return Cursors.SizeNESW;
                    case ResizeDirection.Bottom: return Cursors.SizeNS;
                    case ResizeDirection.BottomRight: return Cursors.SizeNWSE;
                    case ResizeDirection.Left: return Cursors.SizeWE;
                    case ResizeDirection.Right: return Cursors.SizeWE;
                    default: throw new InvalidOperationException("Unknown resize direction");
                }
            }
        }

    }

    public enum ResizeDirection
    {
        Top = 0x1,
        Left = 0x2,
        Right = 0x4,
        Bottom = 0x8,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right
    }
}
