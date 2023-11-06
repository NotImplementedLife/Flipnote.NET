using FlipnoteDotNet.Commons.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FlipnoteDotNet.VisualComponentsEditor
{
    public abstract class VisualComponent
    {
        #region Fields
        private readonly BitmapKey pBitmapKey;
        private Size pSize;
        private Point pLocation;
        private Point[] pPolygonBounds=new Point[4];
        private float pRotation = 0;        
        private bool pDirtyTransform = true;
        private Matrix pTransform = new Matrix();
        private Rectangle pBounds;

        #endregion Fields


        #region Properties
        public BitmapKey BitmapKey => pBitmapKey;
        public Size Size
        {
            get => pSize;
            set
            {
                pSize = value;
                OnResize();
            }
        }

        public Point Location
        {
            get => pLocation;
            set
            {
                pLocation = value;
                OnMoved();
            }
        }
               
        public float Rotation { get => pRotation; set { pRotation = value; pDirtyTransform = true; } }        
        public Matrix Transform { get => pTransform; }

        public Rectangle Bounds => pBounds;
        
        #endregion Properties
        public void Refresh() => pBitmapKey.Refresh();
        public abstract Bitmap BuildBitmap();
        private void BitmapUpdatedCallback(BitmapKey key) => BitmapChanged?.Invoke(this);

        public VisualComponent(BitmapProcessor bitmapProcessor)
        {
            pBitmapKey = bitmapProcessor.AddBitmap(BuildBitmap, BitmapUpdatedCallback);
        }

        public delegate void OnBitmapChanged(VisualComponent component);
        public event OnBitmapChanged BitmapChanged;

        protected virtual void OnResize()
        {
            UpdateBounds();            
        }

        protected virtual void OnMoved()
        {
            UpdateBounds();
        }

        protected virtual void OnTransformChanged()
        {
            Refresh();
        }

        private void UpdateBounds()
        {
            var r = new Rectangle(pLocation, pSize);
            pPolygonBounds[0] = new Point(r.Left, r.Top);
            pPolygonBounds[1] = new Point(r.Right, r.Top);
            pPolygonBounds[2] = new Point(r.Right, r.Bottom);
            pPolygonBounds[3] = new Point(r.Left, r.Bottom);
            pTransform.TransformPoints(pPolygonBounds);

            int x0 = int.MaxValue, y0 = int.MaxValue, x1 = int.MinValue, y1 = int.MinValue;
            for(int i=0;i<4;i++)
            {                
                if (pPolygonBounds[i].X < x0) x0 = pPolygonBounds[i].X;
                if (pPolygonBounds[i].X > x1) x1 = pPolygonBounds[i].X;
                if (pPolygonBounds[i].Y < y0) y0 = pPolygonBounds[i].Y;
                if (pPolygonBounds[i].Y > y1) y1 = pPolygonBounds[i].Y;
            }
            pBounds = new Rectangle(x0, y0, x1 - x0 + 1, y1 - y0 + 1);
        }

        public void UpdateTransform()
        {            
            pTransform = new Matrix();
            pTransform.RotateAt(pRotation, new PointF(pLocation.X + pSize.Width / 2, pLocation.Y + pSize.Height / 2));
            UpdateBounds();

            if (pDirtyTransform)
            {
                pDirtyTransform = false;

                OnTransformChanged();             
            }
        }        

        public IEnumerable<Point> GetPolygonBounds() => pPolygonBounds;

        private static int HRayIntersect(Point r, Point a, Point b)
        {
            if (r.Y <= Math.Min(a.Y, b.Y) || r.Y >= Math.Max(a.Y, b.Y)) return 0;

            int m = a.Y - b.Y;
            int n = b.X - a.X;
            int p = a.X * b.Y - a.Y * b.X;            

            if (m == 0) return n * r.Y + p == 0 && ((a.X <= r.X && r.X < b.X) || (b.X <= r.X && r.X < a.X)) ? 1 : 0;
            int x = -(n * r.Y + p) / m;

            if (x < r.X) return 0;

            if (x < Math.Min(a.X, b.X) || x > Math.Max(a.X, b.X)) return 0;

            return 1;
            
        }

        public bool HitTest(Point point)
        {            
            if (!pBounds.Contains(point)) return false;
            var s = HRayIntersect(point, pPolygonBounds[0], pPolygonBounds[1])
                + HRayIntersect(point, pPolygonBounds[1], pPolygonBounds[2])
                + HRayIntersect(point, pPolygonBounds[2], pPolygonBounds[3])
                + HRayIntersect(point, pPolygonBounds[3], pPolygonBounds[0]);
            
            return s == 1;
        }
    }
}
