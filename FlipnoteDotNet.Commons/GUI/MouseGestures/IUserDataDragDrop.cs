namespace FlipnoteDotNet.Commons.GUI.MouseGestures
{
    public interface IUserDataDragDrop
    {
        void OnDrag(object sender, DragGestureArgs e);
        void OnDrop(object sender, DropGestureArgs e);
    }    
}
