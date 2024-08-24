namespace FlipnoteDotNet.Drawing.Renderers
{
    public static class VisualRendererUtils
    {
        public static PointF[] GetCorners(int width, int height)
            => new[] { new PointF(0, 0), new PointF(width, 0), new PointF(0, height), new PointF(width, height) };

        public static Rectangle FitInRectangle(PointF[] points)
        {
            float x0 = points[0].X, x1 = points[0].X;
            float y0 = points[0].Y, y1 = points[0].Y;

            for(int i=0;i<points.Length;i++)
            {
                if (points[i].X < x0) x0 = points[i].X;
                if (points[i].X > x1) x1 = points[i].X;
                if (points[i].Y < y0) y0 = points[i].Y;
                if (points[i].Y > y1) y1 = points[i].Y; 
            }

            int left = (int)Math.Floor(x0);
            int top = (int)Math.Floor(y0);
            int width = (int)Math.Ceiling(x1 - x0);
            int height = (int)Math.Ceiling(y1 - y0);

            if (width == 0) width = 1;
            if (height == 0) height = 1;

            return new Rectangle(left, top, width, height);                
        }
    }
}
