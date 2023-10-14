using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.GUI.Canvas.Drawing.Operations
{
    internal class DrawLine : ICanvasGraphicsOperation
    {
        public Pen Pen { get; }
        public Point P1 { get; }
        public Point P2 { get; }

        public DrawLine(Pen pen, Point p1, Point p2)
        {
            Pen = pen;
            P1 = p1;
            P2 = p2;
        }

        public void Execute(CanvasGraphicsRenderer renderer)
        {
            renderer.DrawLine(Pen, P1, P2);
        }
    }
}
