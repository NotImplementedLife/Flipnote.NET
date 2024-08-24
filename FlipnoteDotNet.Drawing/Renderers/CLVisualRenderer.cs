using FlipnoteDotNet.Drawing.Utils;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;

namespace FlipnoteDotNet.Drawing.Renderers
{
    public class CLVisualRenderer : IVisualRenderer
    {
        public CLVisualRenderer() 
        {
            CL.Init();
        }        

        public void RenderTransform(Bitmap source, Matrix3x2 transformMatrix, out Bitmap destBitmap, out Rectangle destBounds, VisualRenderOptions options = default)
        {
            var corners = VisualRendererUtils.GetCorners(source.Width, source.Height);
            using (var transform = new Matrix(transformMatrix))
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

        object Lock = new object();        


        public void OrderedDithering(Bitmap source, Palette palette, int order, out Bitmap dest, int alphaThreshold = 128)
        {
            if (source.PixelFormat != PixelFormat.Format32bppPArgb)
                throw new ArgumentException("Invalid pixel format.");
            byte[] m = BayerMatrices.M[order];
            int n = 1 << order;
            int modN = n - 1;

            var rect = new Rectangle(Point.Empty, source.Size);
            dest = new Bitmap(source);
            var bmpData = dest.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppPArgb);
            int[] pixels = GC.AllocateArray<int>(rect.Width * rect.Height);
            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);
            CLOrderedDithering.Call(pixels, source.Width, source.Height, m, order, palette, alphaThreshold);
            Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            dest.UnlockBits(bmpData);          
        }
    }
}
