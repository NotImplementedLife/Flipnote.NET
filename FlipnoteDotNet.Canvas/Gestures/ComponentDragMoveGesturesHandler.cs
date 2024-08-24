using FlipnoteDotNet.Canvas.Components;
using FlipnoteDotNet.Core.MouseGestures;

namespace FlipnoteDotNet.Canvas.Gestures
{
    internal class ComponentDragMoveGesturesHandler : MouseMoveZoomGesturesHandler
    {
        private readonly CanvasControl Control;
        internal CanvasComponent Component;
        private int X0, Y0;

        private CanvasTransform OldTransform;

        public ComponentDragMoveGesturesHandler(CanvasControl control) :
            base(control.AcquireCoords, control.UpdateCoords)
        {
            Control = control;            
        }

        protected override void OnDragStart(DragGestureArgs e)
        {            
            X0 = Component.Transform.X;
            Y0 = Component.Transform.Y;
            OldTransform = Component.Transform;
        }

        protected override void OnDrag(DragGestureArgs e)
        {            
            var startPoint = Control.PointScreenToCanvasF(e.StartLocation);
            var currentPoint = Control.PointScreenToCanvasF(e.CurrentLocation);

            var deltaX = currentPoint.X - startPoint.X;
            var deltaY = currentPoint.Y - startPoint.Y;

            var transform = Component.Transform;
            Component.Transform = transform with
            {
                X = (int)(X0 + deltaX),
                Y = (int)(Y0 + deltaY)
            };

            Component.Invalidate();
        }

        protected override void OnDrop(DropGestureArgs e)
        {
            base.OnDrop(e);
            Control.TriggerComponentTransformChanged(Component, OldTransform, Component.Transform);
        }


    }
}
