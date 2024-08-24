using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.Utils;
using System.Drawing.Imaging;

namespace FlipnoteDotNet.App.Data.Assets
{
    public class StaticBitmap : Asset
    {
        private readonly Stream ImageStream = new MemoryStream();

        public StaticBitmap(Bitmap bitmap, string name = "", bool disposeBitmap=false) : base(bitmap.CreateThumbnail(64, 64), name)
        {
            bitmap.Save(ImageStream, ImageFormat.Png);
            if (disposeBitmap)
                bitmap.Dispose();            
        }

        public override FlipnoteCanvasComponent CreateCanvasComponent()
        {
            var bitmap = new Bitmap(ImageStream);
            return new FlipnoteSprite(this, bitmap);
        }
    }
}
