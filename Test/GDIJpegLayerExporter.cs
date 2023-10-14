using PPMLib.Data;
using PPMLib.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Encoder = System.Drawing.Imaging.Encoder;

namespace Test
{
    internal class GDIJpegLayerExporter : JpegLayerExporter
    {
        public GDIJpegLayerExporter(byte[] commonKey, byte[] iv = null) : base(commonKey, iv)
        {
        }

        public Bitmap LayerToBitmap(FlipnoteFrameLayer layer)
        {
            uint[] data = new uint[256 * 192];
            int i = 0;
            for (int y = 0; y < 192; y++)
                for (int x = 0; x < 256; x++)
                    data[i++] = layer[x, y] != 0 ? 0xFF000000 : 0xFFFFFFFF;
            unsafe
            {
                fixed (uint* ptr = data)
                    return new Bitmap(256, 192, 4 * 256, PixelFormat.Format32bppRgb, new IntPtr(ptr));
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
                if (codec.FormatID == format.Guid)
                    return codec;         
            return null;
        }

        protected override byte[] GetLayerJpg(FlipnoteFrameLayer layer)
        {
            using (var bitmap = LayerToBitmap(layer))
            using (var bmpScaled = new Bitmap(bitmap, 640, 480)) 
            using (var ms = new MemoryStream())
            {
                var encParams = new EncoderParameters(1);
                encParams.Param[0] = new EncoderParameter(Encoder.Quality, 100L);

                bmpScaled.Save(ms, GetEncoder(ImageFormat.Jpeg), encParams);
                return ms.ToArray();
            }
        }

        protected override byte[] GetLayerJpgThumbnail(FlipnoteFrameLayer layer)
        {
            using (var bitmap = LayerToBitmap(layer))
            using (var bmpScaled = new Bitmap(bitmap, 160, 120))
            using (var ms = new MemoryStream()) 
            {
                bmpScaled.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}
