namespace FlipnoteDotNet.Canvas
{
    public class CanvasFrameUpdatedEventArgs : EventArgs
    {
        public Bitmap Bitmap { get; }
        public bool DisposeBitmap { get; set; } = true;

        public CanvasFrameUpdatedEventArgs(Bitmap bitmap)
        {
            Bitmap = bitmap;
        }
    }
}
