using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.Utils
{
    internal class AsyncBitmap
    {
        public object Locker { get; } = new object();

        public Bitmap DisplayBitmap { get; private set; }
        public Action<Graphics> RenderMethod { get; }
        public bool IsBusy { get; private set; }

        public AsyncBitmap(Action<Graphics> renderMethod)
        {
            RenderMethod = renderMethod;
        }

        public void Redraw(int width, int height)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format32bppPArgb);           
            var g = Graphics.FromImage(bmp);
            g.Clip = new Region(new Rectangle(Point.Empty, bmp.Size));
            Task.Run(() =>
            {                
                IsBusy = true;                
                RenderMethod?.Invoke(g);                
                g.Flush(FlushIntention.Sync);
                g.Dispose();

                lock (Locker)
                {
                    DisplayBitmap?.Dispose();
                    DisplayBitmap = bmp;
                }                
                
                Ready?.Invoke(this, new EventArgs());                                                
                IsBusy = false;
            });
        }

        public event EventHandler Ready;
    }
}
