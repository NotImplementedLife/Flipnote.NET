namespace FlipnoteDotNet.Canvas.Components
{
    public class CanvasImage : CanvasComponent
    {
        public Bitmap Bitmap { get; }
        public CanvasImage(Bitmap bitmap, string name="") : base(name, bitmap.Width, bitmap.Height, false) 
        {
            Bitmap = bitmap;
        }
        public override void Draw(Graphics g)
        {
            g.DrawImageUnscaled(Bitmap, 0, 0);
        }

        public override CanvasComponent Clone() => new CanvasImage(Bitmap.Clone() as Bitmap, Name)
        {
            Transform = Transform
        };
    }
}
