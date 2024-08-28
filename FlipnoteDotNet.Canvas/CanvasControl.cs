using FlipnoteDotNet.Canvas.Components;
using FlipnoteDotNet.Canvas.Gestures;
using FlipnoteDotNet.Core.MouseGestures;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace FlipnoteDotNet.Canvas
{
    public partial class CanvasControl : UserControl
    {
        private readonly CanvasModel fCanvasModel = new CanvasModel(256, 192);
        public CanvasModel CanvasModel => fCanvasModel;
        private Bitmap fCanvasBuffer = null;

        private MouseGesturesHandler MouseGesturesHandler = null;

        private readonly MouseMoveZoomGesturesHandler CanvasMoveZoomGesturesHandler;
        private readonly ComponentDragMoveGesturesHandler ComponentDragMoveGesturesHandler;
        private readonly ControlPointDragMoveGesturesHandler ControlPointDragMoveGesturesHandler;

        private readonly SelectionManager SelectionManager = new SelectionManager();        

        public CanvasControl()
        {
            InitializeComponent();

            fCanvasModel.FrameUpdated += FCanvasModel_FrameUpdated;
            fCanvasModel.BindingListChanged += FCanvasModel_BindingListChanged;

            CanvasMoveZoomGesturesHandler = new MouseMoveZoomGesturesHandler(AcquireCoords, UpdateCoords);
            CanvasMoveZoomGesturesHandler.Click += MouseMoveZoomGesturesHandler_Click;

            ComponentDragMoveGesturesHandler = new ComponentDragMoveGesturesHandler(this);
            ControlPointDragMoveGesturesHandler = new ControlPointDragMoveGesturesHandler(this);

            SetMouseGestures(CanvasMoveZoomGesturesHandler);

            SelectionManager.SelectionChanged += SelectionManager_SelectionChanged;                       
        }

        private void FCanvasModel_BindingListChanged(object sender, EventArgs e)
        {
            SelectionManager.ClearSelection();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode)
                CanvasModel.StartProcessor();

        }

        internal (float X, float Y, float Scale) AcquireCoords() => (fCanvasOffset.X, fCanvasOffset.Y, fCanvasScale);
        internal void UpdateCoords(float x, float y, float scale)
        {
            fCanvasOffset = new PointF(x, y);
            fCanvasScale = scale;
            Invalidate();
        }

        private void FCanvasModel_FrameUpdated(object sender, CanvasFrameUpdatedEventArgs e)
        {
            //Invoke(() =>
            {
                var old = fCanvasBuffer;
                fCanvasBuffer = e.Bitmap;
                old?.Dispose();
                e.DisposeBitmap = false;
                Invalidate();
            }//);
        }

        private PointF fCanvasOffset = PointF.Empty;
        private float fCanvasScale = 1;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);            
            e.Graphics.ScaleTransform(fCanvasScale, fCanvasScale, MatrixOrder.Append);
            e.Graphics.TranslateTransform(fCanvasOffset.X, fCanvasOffset.Y, MatrixOrder.Append);

            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            using (var pen = new Pen(Brushes.Black, 2 / fCanvasScale)) 
                e.Graphics.DrawRectangle(pen, 0, 0, 256, 192);
            e.Graphics.FillRectangle(Brushes.White, 0, 0, 256, 192);
            if (fCanvasBuffer != null)
                e.Graphics.DrawImageUnscaled(fCanvasBuffer, 0, 0);
            SelectionManager.OnPaint(e.Graphics);

            if (DesignMode) 
            {
                e.Graphics.DrawString("[Design]", Font, Brushes.Blue, Point.Empty);
            }
        }

        private void SetMouseGestures(MouseGesturesHandler gesturesHandler)
        {
            if (MouseGesturesHandler == gesturesHandler)
                return;
            MouseGesturesHandler?.DetachTarget();
            MouseGesturesHandler = gesturesHandler;
            MouseGesturesHandler?.AttachTarget(this);

            Debug.WriteLine($"MGH: {MouseGesturesHandler?.GetType()?.Name ?? "null"}");
        }

        internal Point PointScreenToCanvas(Point p)
        {
            int canvasX = (int)((p.X - fCanvasOffset.X) / fCanvasScale);
            int canvasY = (int)((p.Y - fCanvasOffset.Y) / fCanvasScale);
            return new Point(canvasX, canvasY);
        }

        internal PointF PointScreenToCanvasF(PointF p)
        {
            float canvasX = (p.X - fCanvasOffset.X) / fCanvasScale;
            float canvasY = (p.Y - fCanvasOffset.Y) / fCanvasScale;
            return new PointF(canvasX, canvasY);
        }

        private void MouseMoveZoomGesturesHandler_Click(object sender, ClickGestureArgs e)
        {
            var canvasPoint = PointScreenToCanvas(e.Location);
            var component = fCanvasModel.GetComponentAtLocation(canvasPoint);
            if (component != null)
            {
                SelectionManager.ClearSelection();
                SelectionManager.ToggleSelection(component);
            }
            else
            {
                SelectionManager.ClearSelection();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var canvasPoint = PointScreenToCanvas(e.Location);
            if (SelectionManager.ControlPointHitTest(canvasPoint, out var selectionState)) 
            {
                ControlPointDragMoveGesturesHandler.SelectionState = selectionState;
                SetMouseGestures(ControlPointDragMoveGesturesHandler);
                goto end;
            }

            var comp = CanvasModel.GetComponentAtLocation(canvasPoint);
            Debug.WriteLine($"MouseDown on: {comp?.ToString() ?? "null"}");
            if (comp != null && SelectionManager.IsSelected(comp)) 
            {                                
                ComponentDragMoveGesturesHandler.Component = comp;
                SetMouseGestures(ComponentDragMoveGesturesHandler);
                goto end;
            }

            SetMouseGestures(CanvasMoveZoomGesturesHandler);
        end:
            base.OnMouseDown(e);            
        }

        private void SelectionManager_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
            Invalidate();
        }

        public void ClearSelection() => SelectionManager.ClearSelection();
        public void SelectSingle(CanvasComponent component) => SelectionManager.SelectSingle(component);

        public CanvasComponent SelectedComponent => SelectionManager.GetSelectedComponents().FirstOrDefault();

        public event EventHandler SelectionChanged;

        public delegate void OnTransformChanged(object sender, CanvasComponent component, CanvasTransform oldTransform, CanvasTransform newTransform);
        public event OnTransformChanged ComponentTransformChanged;

        internal void TriggerComponentTransformChanged(CanvasComponent component, CanvasTransform oldTransform, CanvasTransform newTransform)
        {
            ComponentTransformChanged?.Invoke(this, component, oldTransform, newTransform);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

    }
}
