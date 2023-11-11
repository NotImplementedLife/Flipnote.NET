using System.Drawing;

namespace FlipnoteDotNet.Commons.GUI.MouseGestures
{
    public delegate void ClickGesture(object sender, ClickGestureArgs e);
    public delegate void DragStartGesture(object sender, DragGestureArgs e);
    public delegate void DragGesture(object sender, DragGestureArgs e);
    public delegate void DropGesture(object sender, DropGestureArgs e);
    public delegate void ZoomGesture(object sender, ZoomGestureArgs e);

    public class ZoomGestureArgs
    {
        public Point CursorLocation { get; }
        public int Factor { get; }
        public ZoomGestureArgs(Point location, int factor)
        {
            CursorLocation = location;
            Factor = factor;
        }
    }

    public class ClickGestureArgs
    {
        public Point Location { get; }

        public ClickGestureArgs(Point location)
        {
            Location = location;
        }
    }

    public class DragGestureArgs
    {
        public Point StartLocation { get; }
        public Point CurrentLocation { get; }
        public object UserData { get; set; }
        public Point DeltaLocation { get; }

        public bool IsCanceled { get; private set; } = false;

        public DragGestureArgs(Point startLocation, Point currentLocation)
        {
            StartLocation = startLocation;
            CurrentLocation = currentLocation;
            DeltaLocation = new Point(CurrentLocation.X - StartLocation.X, CurrentLocation.Y - StartLocation.Y);
        }
        public void Cancel() => IsCanceled = true;
    }

    public class DropGestureArgs
    {
        public Point StartLocation { get; }
        public Point CurrentLocation { get; }
        public object UserData { get; }
        public DropGestureArgs(Point startLocation, Point currentLocation, object userData)
        {
            StartLocation = startLocation;
            CurrentLocation = currentLocation;
            UserData = userData;
        }
    }
}
