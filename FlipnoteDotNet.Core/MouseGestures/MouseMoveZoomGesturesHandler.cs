using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Core.MouseGestures
{
    public class MouseMoveZoomGesturesHandler : MouseGesturesHandler
    {
        private readonly Func<(float X, float Y, float Scale)> AcquireCoords;
        private readonly Action<float, float, float> UpdateCoords;

        public MouseMoveZoomGesturesHandler(Func<(float X, float Y, float Scale)> acquireCoords, Action<float, float, float> updateCoords)
        {
            AcquireCoords = acquireCoords;
            UpdateCoords = updateCoords;

            if (AcquireCoords == null) throw new ArgumentNullException("AcquireCoords is null");
            if (UpdateCoords == null) throw new ArgumentNullException("UpdateCoords is null");
        }

        float startX, startY, startScale;

        protected override void OnDragStart(DragGestureArgs e)
        {
            (startX, startY, startScale) = AcquireCoords();
        }        

        protected override void OnDrag(DragGestureArgs e)
        {
            float x = startX + e.DeltaLocation.X;
            float y = startY + e.DeltaLocation.Y;            
            UpdateCoords(x, y, startScale);
        }        

        protected override void OnZoom(ZoomGestureArgs e)
        {
            (startX, startY, startScale) = AcquireCoords();
            float px = (e.CursorLocation.X - startX) / startScale;
            float py = (e.CursorLocation.Y - startY) / startScale;            
            float scale = startScale + e.Factor * 0.2f;
            if (scale < 1) scale = 1;
            if (scale > 5) scale = 5;

            float tx = px * (startScale - scale) + startX;
            float ty = py * (startScale - scale) + startY;

            UpdateCoords(tx, ty, scale);
        }        
    }
}
