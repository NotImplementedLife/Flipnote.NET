using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Canvas.Drawing.Operations;

namespace FlipnoteDotNet.GUI.Canvas.Drawing
{
    public class CanvasGraphics
    {
        internal CanvasGraphics(CanvasGraphicsRenderer renderer)
        {
            Renderer = renderer;
        }

        CanvasGraphicsRenderer Renderer { get; }
        Queue<ICanvasGraphicsOperation> PendingOperations { get; } = new Queue<ICanvasGraphicsOperation>();

        public void Flush()
        {

            PendingOperations.DequeueAll().ForEach(_ =>
            {
                Debug.WriteLine(_);
                _.Execute(Renderer);
            });
        }

        public void DrawLine(Pen pen, Point p1, Point p2)
            => PendingOperations.Enqueue(new DrawLine(pen, p1, p2));

        public void DrawLine(Pen pen, int x1, int y1, int x2, int y2)
            => DrawLine(pen, new Point(x1, y1), new Point(x2, y2));

        public void DrawRectangle(Pen pen, Rectangle rectangle)
            => PendingOperations.Enqueue(new DrawRectangle(pen, rectangle));

        public void DrawRectangle(Pen pen, Point point, Size size)
            => DrawRectangle(pen, new Rectangle(point, size));

        public void DrawRectangle(Pen pen, int x, int y, int width, int height)
            => DrawRectangle(pen, new Rectangle(x, y, width, height));


    }
}
