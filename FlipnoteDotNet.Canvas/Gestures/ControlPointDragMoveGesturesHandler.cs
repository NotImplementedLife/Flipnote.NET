using FlipnoteDotNet.Canvas.Selections;
using FlipnoteDotNet.Core.MouseGestures;
using System.ComponentModel;

namespace FlipnoteDotNet.Canvas.Gestures
{
    internal class ControlPointDragMoveGesturesHandler : MouseMoveZoomGesturesHandler
    {
        private readonly CanvasControl Control;
        internal SelectionState SelectionState;

        private CanvasTransform OldTransform;

        public ControlPointDragMoveGesturesHandler(CanvasControl control) : base(control.AcquireCoords, control.UpdateCoords)
        {
            Control = control;             
        }

        protected override void OnDragStart(DragGestureArgs e)
        {
            SelectionState.CaptureBaseState();
            OldTransform = SelectionState.Component.Transform;
        }

        protected override void OnDrag(DragGestureArgs e)
        {
            var canvasPoint = Control.PointScreenToCanvasF(e.CurrentLocation);
            SelectionState.OnActivePointMoved(canvasPoint);
            SelectionState.Component.Invalidate();
        }

        protected override void OnDrop(DropGestureArgs e)
        {
            SelectionState.UpdateComponentState();
            var component = SelectionState.Component;
            Control.TriggerComponentTransformChanged(component, OldTransform, component.Transform);
        }

    }
}
