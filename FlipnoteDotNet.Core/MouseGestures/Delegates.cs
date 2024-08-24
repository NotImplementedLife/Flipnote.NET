namespace FlipnoteDotNet.Core.MouseGestures
{
    public delegate void ClickGesture(object sender, ClickGestureArgs e);
    public delegate void DragStartGesture(object sender, DragGestureArgs e);
    public delegate void DragGesture(object sender, DragGestureArgs e);
    public delegate void DropGesture(object sender, DropGestureArgs e);
    public delegate void ZoomGesture(object sender, ZoomGestureArgs e);    
}
