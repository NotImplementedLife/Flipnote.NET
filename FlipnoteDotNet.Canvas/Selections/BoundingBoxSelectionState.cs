using FlipnoteDotNet.Canvas.Components;

namespace FlipnoteDotNet.Canvas.Selections
{
    public class BoundingBoxSelectionState : SelectionState
    {
        public BoundingBoxSelectionState(CanvasComponent component) : base(component, 4)
        {
        }

        public override void CaptureBaseState()
        {
            ControlPoints[0] = new Point(0, 0);
            ControlPoints[1] = new Point(Component.Width, 0);
            ControlPoints[2] = new Point(Component.Width, Component.Height);
            ControlPoints[3] = new Point(0, Component.Height);
            using (var transform = Component.CloneDirectTransform())
                transform.TransformPoints(ControlPoints);
        }

        private static PointF Symmetric(float a, float b, float c, PointF p)
        {
            var d = 2 * (a * p.X + b * p.Y + c);
            return new PointF(p.X - d * a, p.Y - d * b);
        }

        private static float Distance(PointF a, PointF b)
        {
            float dx = a.X - b.X, dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        private static float AddEpsilonIfNeeded(float x, float eps = 1e-3f)
        {
            if (x >= 0 && x < eps) return x + eps;
            if (x < 0 && x > -eps) return x - eps;
            return x;
        }        

        private static int Orientation(float cos, float sin, PointF p0, PointF p1)
        {
            var b = Math.Abs(cos) >= Math.Abs(sin)
                ? (p1.X - p0.X) / cos
                : (p1.Y - p0.Y) / sin;
            return b >= 0 ? 1 : -1;
        }

        public override void OnActivePointMoved(PointF newPoint)
        {
            var angle = Component.Transform.Rotation;
            var oldXOr = Orientation(angle.CosValue, angle.SinValue, ControlPoints[0], ControlPoints[1]);
            var oldYOr = Orientation(angle.SinValue, -angle.CosValue, ControlPoints[1], ControlPoints[2]);
            ControlPoints[ActivePointIndex] = newPoint;
            int oppositePointIndex = ActivePointIndex < 2 ? ActivePointIndex + 2 : ActivePointIndex - 2;
            PointF p0 = newPoint;
            PointF p1 = ControlPoints[oppositePointIndex];
            float mx = (p0.X + p1.X) / 2;
            float my = (p0.Y + p1.Y) / 2;

            float a = - Component.Transform.Rotation.SinValue;
            float b = Component.Transform.Rotation.CosValue;
            float c = -a * mx - b * my;

            ControlPoints[3 - ActivePointIndex] = Symmetric(a, b, c, p0);
            ControlPoints[3 - oppositePointIndex] = Symmetric(a, b, c, p1);

            var newWidth = Distance(ControlPoints[0], ControlPoints[1]);
            var newHeight = Distance(ControlPoints[1], ControlPoints[2]);
            var newXOr = Orientation(angle.CosValue, angle.SinValue, ControlPoints[0], ControlPoints[1]);
            var newYOr = Orientation(angle.SinValue, -angle.CosValue, ControlPoints[1], ControlPoints[2]);

            var transform = Component.Transform;

            var scaleX = Math.Sign(transform.ScaleX) * AddEpsilonIfNeeded(newWidth / Component.Width);
            var scaleY = Math.Sign(transform.ScaleY) * AddEpsilonIfNeeded(newHeight / Component.Height);
            

            if (newXOr != oldXOr && newXOr != 0) scaleX = -scaleX;
            if (newYOr != oldYOr && newYOr != 0) scaleY = -scaleY;

            //Debug.WriteLine($"fScale: {scaleX}, {scaleY}");

            (var ax, var ay) = (transform.AnchorX * Component.Width, transform.AnchorY * Component.Height);
            (var cos, var sin) = (transform.Rotation.CosValue, transform.Rotation.SinValue);            

            var trX = ControlPoints[0].X - ax + ax * cos * scaleX - ay * sin * scaleY;
            var trY = ControlPoints[0].Y - ay + ax * sin * scaleX + ay * cos * scaleY;

            Detach();
            Component.Transform = transform with
            {
                ScaleX = scaleX,
                ScaleY = scaleY,
                X = (int)trX,
                Y = (int)trY
            };
            Attach();
        }

        public override void OnPaint(Graphics g)
        {
            var ax = Component.Transform.AnchorX * Component.Width;
            var ay = Component.Transform.AnchorY * Component.Height;
            var points = new Point[]
            {
                new Point(0,0),
                new Point(Component.Width,0),
                new Point(Component.Width,Component.Height),
                new Point(0,Component.Height),                
            };

            var anchor = new PointF[] { new PointF(ax, ay) };

            using (var transform = Component.CloneDirectTransform())
            {
                transform.TransformPoints(points);
                transform.TransformPoints(anchor);
            }                

            g.DrawPolygon(Pens.DodgerBlue, points);

            for(int i=0;i<ControlPoints.Length;i++) 
            {
                SelectionRenderer.DrawPoint(g, ControlPoints[i]);
            }

            SelectionRenderer.DrawPoint(g, anchor[0]);            
        }

        public override void UpdateComponentState()
        {
            base.Detach();

            //Component.Transform = 

            base.Attach();
        }
    }
}
