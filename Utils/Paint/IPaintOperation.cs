using FlipnoteDotNet.GUI.MouseGestures;

namespace FlipnoteDotNet.Utils.Paint
{
    public interface IPaintOperation
    {
        IPaintDevice Device { get; set; }
        PaintContext PaintContext { get; set; }
        void Commit();
        void Cancel();
        void OnDragStart(object sender, DragGestureArgs e);
        void OnDrag(object sender, DragGestureArgs e);
        void OnDrop(object sender, DropGestureArgs e);
        void OnClick(object sender, ClickGestureArgs e);        
    }
}
