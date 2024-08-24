using System.Drawing.Imaging;

namespace FlipnoteDotNet.Utils
{
    public static class Images
    {
        public static Bitmap CreateThumbnail(this Bitmap bmp, int thumbnailWidth, int thumbnailHeight)
        {
            var thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight, PixelFormat.Format32bppPArgb);
            var scale = Math.Min(1f * thumbnailWidth / bmp.Width, 1f * thumbnailHeight / bmp.Height);
            var bw = scale * bmp.Width;
            var bh = scale * bmp.Height;
            var bx = (thumbnailWidth - bw) / 2;
            var by = (thumbnailHeight - bh) / 2;
            using (var g = Graphics.FromImage(thumbnail))
                g.DrawImage(bmp, bx, by, bw, bh);            
            return thumbnail;
        }
    }
}
