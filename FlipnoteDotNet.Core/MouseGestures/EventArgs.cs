namespace FlipnoteDotNet.Core.MouseGestures
{
    public class ZoomGestureArgs
    {
        public Point CursorLocation { get; }
        public int Factor { get; }
        public ZoomGestureArgs(Point location, int factor)
        {
            CursorLocation = location;
            Factor = factor;
        }

        public override string ToString() => $"ZoomGestureArgs{{" +
            $"CursorLocation={CursorLocation}, " +
            $"Factor={Factor}" +
            $"}}";        
    }

    public class ClickGestureArgs
    {
        public Point Location { get; }

        public ClickGestureArgs(Point location)
        {
            Location = location;
        }

        public override string ToString() => $"ClickGestureArgs{{Location={Location}}}";        
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

        public override string ToString() => $"DragGestureArgs{{" +
            $"StartLocation={StartLocation}, " +
            $"CurrentLocation={CurrentLocation}, " +
            $"UserData={UserData}, " +
            $"DeltaLocation={DeltaLocation}}}";
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

        public override string ToString() => $"DropGestureArgs{{" +
            $"StartLocation={StartLocation}, " +
            $"CurrentLocation={CurrentLocation}, " +
            $"UserData={UserData}}}";
    }
}
