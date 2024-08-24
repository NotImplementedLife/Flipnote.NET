using FlipnoteDotNet.Canvas.Components;

namespace FlipnoteDotNet.Canvas.Selections
{
    public abstract class SelectionState
    {
        internal readonly CanvasComponent Component;
        protected readonly PointF[] ControlPoints;
        protected int ActivePointIndex = -1;
        protected PointF ActivePoint0 = Point.Empty;

        public SelectionState(CanvasComponent component, int pointsCount)
        {
            Component = component;
            ControlPoints = new PointF[pointsCount];
            Component.TransformChanged += Component_TransformChanged;
        }

        private void Component_TransformChanged(object sender, EventArgs e)
        {
            CaptureBaseState();
        }

        public void Detach() => Component.TransformChanged -= Component_TransformChanged;
        public void Attach() => Component.TransformChanged += Component_TransformChanged;

        public abstract void CaptureBaseState();
        public abstract void OnActivePointMoved(PointF newPoint);
        public abstract void UpdateComponentState();
        public abstract void OnPaint(Graphics g);

        public int HitTest(PointF point)
        {
            for (int i = 0; i < ControlPoints.Length; i++)
            {
                if (Utils.DistanceSquared(point, ControlPoints[i]) < 6f) 
                    return i;
            }
            return -1;
        }

        internal void SetActivePoint(int index)
        {
            ActivePointIndex = index;
            if (index >= 0) ActivePoint0 = ControlPoints[index];
        }

        

    }
}
