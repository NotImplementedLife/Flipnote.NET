using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Canvas.Drawing;
using FlipnoteDotNet.GUI.MouseGestures;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Canvas
{
    public partial class CanvasSpaceControl : UserControl
    {
        public List<ICanvasComponent> CanvasComponents { get; } = new List<ICanvasComponent>();

        MouseGesturesHandler MouseGesturesHandler = new MouseGesturesHandler();

        [Browsable(false)]        
        public Point CanvasViewLocation { get; set; } = new Point(0, 0);

        [Browsable(false)]
        public Point CanvasViewScale => new Point(CanvasViewScaleFactor, CanvasViewScaleFactor);

        [Browsable(false)]
        [DefaultValue(100)]
        public int CanvasViewScaleFactor { get; set; } = 100;

        public CanvasSpaceControl()
        {
            InitializeComponent();

            MouseGesturesHandler.DragStart += MouseGesturesHandler_DragStart;
            MouseGesturesHandler.Drag += MouseGesturesHandler_Drag;
            MouseGesturesHandler.Drop += MouseGesturesHandler_Drop;
            MouseGesturesHandler.Click += MouseGesturesHandler_Click;
            MouseGesturesHandler.Zoom += MouseGesturesHandler_Zoom;
            MouseGesturesHandler.AttachTarget(this);

            OldSize = Size;
        }        

        class CanvasDragMoveData
        {
            public Point CanvasViewLocation { get; }

            public CanvasDragMoveData(Point canvasViewLocation)
            {
                CanvasViewLocation = canvasViewLocation;
            }
        }

        private Point ScreenToCanvas(Point p)
        {
            var x = (p.X - CanvasViewLocation.X) * 100 / CanvasViewScaleFactor;
            var y = (p.Y - CanvasViewLocation.Y) * 100 / CanvasViewScaleFactor;
            return new Point(x, y);
        }

        private Point CanvasToScreen(Point p)
        {
            var x = CanvasViewLocation.X + CanvasViewScaleFactor * p.X / 100;
            var y = CanvasViewLocation.Y + CanvasViewScaleFactor * p.Y / 100;
            return new Point(x, y);
        }

        private void ScaleAroundPoint(Point screenPos, int newScaleFactor)
        {            
            var x = CanvasViewLocation.X * newScaleFactor / CanvasViewScaleFactor + screenPos.X * (CanvasViewScaleFactor - newScaleFactor) / CanvasViewScaleFactor;
            var y = CanvasViewLocation.Y * newScaleFactor / CanvasViewScaleFactor + screenPos.Y * (CanvasViewScaleFactor - newScaleFactor) / CanvasViewScaleFactor;
            CanvasViewLocation = new Point(x, y);
            CanvasViewScaleFactor = newScaleFactor;
        }

        private void MouseGesturesHandler_Zoom(object sender, ZoomGestureArgs e)
        {            
            ScaleAroundPoint(e.CursorLocation, (CanvasViewScaleFactor + e.Factor).Clamp(25, 1000));            
            Invalidate();
        }

        private void MouseGesturesHandler_Click(object sender, ClickGestureArgs e)
        {            
            Debug.WriteLine("Click");
        }

        private void MouseGesturesHandler_DragStart(object sender, DragGestureArgs e)
        {
            Debug.WriteLine($"Drag start {e.StartLocation}; {e.CurrentLocation}");

            e.UserData = new CanvasDragMoveData(CanvasViewLocation);            
        }

        private void MouseGesturesHandler_Drag(object sender, DragGestureArgs e)
        {
            Debug.WriteLine($"Drag {e.StartLocation}; {e.CurrentLocation}");

            var data = e.UserData as CanvasDragMoveData;

            var location = data.CanvasViewLocation;
            location.Offset(e.DeltaLocation);
            CanvasViewLocation = location;

            Invalidate();
        }

        private void MouseGesturesHandler_Drop(object sender, DropGestureArgs e)
        {
            Debug.WriteLine($"Drop {e.StartLocation}; {e.CurrentLocation}");
        }

        private void CanvasSpaceControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            var canvasRenderer = new CanvasGraphicsRenderer(e.Graphics, CanvasViewScale, CanvasViewLocation);
            var canvasGraphics = new CanvasGraphics(canvasRenderer);
            CanvasComponents.ForEach(_ => _.OnPaint(canvasGraphics));
            canvasGraphics.Flush();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        Size OldSize;
        private void CanvasSpaceControl_Resize(object sender, System.EventArgs e)
        {
            var x = CanvasViewLocation.X + Size.Width / 2 - OldSize.Width / 2;
            var y = CanvasViewLocation.Y + Size.Height / 2 - OldSize.Height / 2;
            OldSize = Size;
            CanvasViewLocation = new Point(x, y);
            Invalidate();
        }
    }
}
