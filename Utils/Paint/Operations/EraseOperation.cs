using FlipnoteDotNet.GUI.MouseGestures;
using System.Drawing;

namespace FlipnoteDotNet.Utils.Paint.Operations
{
    internal class EraseOperation : IPaintOperation
    {
        public IPaintDevice Device { get; set; }
        public PaintContext PaintContext { get; set; } 

        public void Cancel() { }

        public void Commit() { }    

        public void OnClick(object sender, ClickGestureArgs e)
        {
            Device.SetPixel(e.Location.X, e.Location.Y, 0);
            Device.UpdateDevice();
        }

        public void OnDrag(object sender, DragGestureArgs e)
        {
            var p1 = (Point)e.UserData;
            var p2 = e.CurrentLocation;
            Algorithms.Bresenham(p1, p2, p => Device.SetPixel(p.X, p.Y, 0));
            Device.UpdateDevice();
            e.UserData = p2;
        }

        public void OnDragStart(object sender, DragGestureArgs e)
        {
            e.UserData = e.StartLocation;
        }

        public void OnDrop(object sender, DropGestureArgs e)
        {
            
        }
    }
}
