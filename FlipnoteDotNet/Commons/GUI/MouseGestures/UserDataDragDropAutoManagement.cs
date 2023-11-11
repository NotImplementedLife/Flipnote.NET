namespace FlipnoteDotNet.Commons.GUI.MouseGestures
{
    public static class UserDataDragDropAutoManagement
    {
        public static void MouseGestures_Drag(object sender, DragGestureArgs e)
        {
            (e.UserData as IUserDataDragDrop)?.OnDrag(sender, e);            
        }
        public static void MouseGestures_Drop(object sender, DropGestureArgs e)
        {
            (e.UserData as IUserDataDragDrop)?.OnDrop(sender, e);
        }

    }
}
