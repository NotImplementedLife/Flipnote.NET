using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Storage;
using FlipnoteDotNet.App.Storage.Assets;
using FlipnoteDotNet.Utils;
using System.Drawing.Imaging;

namespace FlipnoteDotNet.App.Data.Assets
{
    public class StaticBitmap : Asset
    {
        private readonly byte[] ImageBytes;

        public StaticBitmap(byte[] imageBytes, byte[] thumbnailBytes, string name = "") 
            : base(Bytes2Bitmap(thumbnailBytes), name)
        {
            ImageBytes = imageBytes;
        }

        public StaticBitmap(Bitmap bitmap, string name = "", bool disposeBitmap=false) : base(bitmap.CreateThumbnail(64, 64), name)
        {
            using(var ms=new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                ImageBytes = ms.ToArray();
            }            
            if (disposeBitmap) 
                bitmap.Dispose();
        }

        public override FlipnoteCanvasComponent CreateCanvasComponent()
        {
            using (var ms = new MemoryStream(ImageBytes, writable: false)) 
            {
                var bitmap = new Bitmap(ms);
                return new FlipnoteSprite(this, bitmap);
            }
        }

        public override AssetDTO ToDTO()
        {
            return new StaticBitmapDTO(Id, Name, GetThumbnailBytes(), ImageBytes);
        }
    }
}
