using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;

namespace FlipnoteDotNet.Drawing.Renderers
{
    public class GDIVisualRenderer : IVisualRenderer
    {        
        public void RenderTransform(Bitmap source, Matrix3x2 transformMatrix, out Bitmap destBitmap, out Rectangle destBounds, VisualRenderOptions options = default)
        {
            var corners = VisualRendererUtils.GetCorners(source.Width, source.Height);
            using(var transform = new Matrix(transformMatrix))
            {
                transform.TransformPoints(corners);
                destBounds = VisualRendererUtils.FitInRectangle(corners);
                if (options.SurfaceBounds.HasValue) destBounds.Intersect(options.SurfaceBounds.Value);
                destBitmap = new Bitmap(destBounds.Width, destBounds.Height, PixelFormat.Format32bppPArgb);

                using (var g = Graphics.FromImage(destBitmap))
                {
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.SmoothingMode = SmoothingMode.HighSpeed;
                    g.MultiplyTransform(transform, MatrixOrder.Append);
                    g.TranslateTransform(-destBounds.Left, -destBounds.Top, MatrixOrder.Append);
                    g.DrawImageUnscaled(source, 0, 0);
                }
            }                    
        }

        public void OrderedDithering(Bitmap source, Palette palette, int order, out Bitmap dest, int alphaThreshold = 128)
        {
            if (source.PixelFormat != PixelFormat.Format32bppPArgb)
                throw new ArgumentException("Invalid pixel format.");

            byte[] m = BayerMatrices.M[order];
            int n = 1 << order;
            int modN = n - 1;

            var rect = new Rectangle(Point.Empty, source.Size);

            var bmpData = source.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            int[] pixels = GC.AllocateUninitializedArray<int>(source.Width * source.Height);
            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);
            source.UnlockBits(bmpData);

            int width = source.Width, height = source.Height;

            int n2over2 = n * n / 2;

            Debug.WriteLine($"Palette spreads: {palette.SpreadR}, {palette.SpreadG}, {palette.SpreadB}");

            int i = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++) 
                {
                    int pixel = pixels[i];
                    int a = (pixel >> 24) & 0xFF;                 
                    if (a < alphaThreshold)
                    {
                        pixels[i++] = 0;
                        continue;
                    }
                    int mapValue = m[((y & modN) << order) | (x & modN)] - n2over2;
                    int r = ((pixel >> 16) & 0xFF) + ((palette.SpreadR * mapValue) >> (2 * order));
                    int g = ((pixel >> 8) & 0xFF) + ((palette.SpreadG * mapValue) >> (2 * order));
                    int b = ((pixel >> 0) & 0xFF) + ((palette.SpreadB * mapValue) >> (2 * order));
                    var color = palette.ClosestColor(r, g, b);
                    pixels[i++] = (int)((color.B << 0) | (color.G << 8) | (color.R << 16) | 0xFF000000);
                }
            }

            dest = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppPArgb);
            bmpData = dest.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);            
            Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            dest.UnlockBits(bmpData);
        }
    }
}
