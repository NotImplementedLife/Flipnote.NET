using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Commons.GUI.MouseGestures;
using PPMLib.Rendering;
using PPMLib.Winforms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace FlipnoteDotNet.VisualComponentsEditor
{
    public partial class VisualComponentsScene : UserControl
    {
        private Point pViewOffset=Point.Empty;
        private int pZoom = 100;
        private MouseGesturesHandler MouseGestures = new MouseGesturesHandler();
        private VisualComponentsManager ComponentsManager = new VisualComponentsManager();
        private List<ControlPoint> ControlPoints = new List<ControlPoint>();
        private ControlPoint HoveredControlPoint = null;

        private static readonly Pen SelectionHighlightPen = new Pen(FlipnoteColors.ThemePrimary, 2);
        private static readonly Pen ComponentOutlinePen = new Pen(Color.FromArgb(64, Color.Black), 2);        


        public VisualComponentsScene()
        {
            InitializeComponent();
            BackColor = SystemColors.ControlDark;
            DoubleBuffered = true;
            Dock = DockStyle.Fill;
            InitializeMouseGestures();
            InitComponentsManager();           
            
        }

        #region ControlPoints

        void LoadControlPoints(VisualComponent c)
        {
            ControlPoints.Clear();
            ControlPoints.Add(new ResizePoint(c, 0, 0));
            ControlPoints.Add(new ResizePoint(c, 0, 1));
            ControlPoints.Add(new ResizePoint(c, 1, 0));
            ControlPoints.Add(new ResizePoint(c, 1, 1));
            ControlPoints.Add(new RotatePoint(c));
        }


        #endregion ControlPoints


        #region ComponentsManager

        private void InitComponentsManager()
        {
            ComponentsManager.SelectionChanged += ComponentsManager_SelectionChanged;

            FlipnoteVisualSource Load(string path)
            {
                using (var bmp = new Bitmap(path))
                {
                    var fvs = new FlipnoteVisualSource(bmp.Width, bmp.Height);

                    for (int y = 0; y < bmp.Height; y++)
                    {
                        for (int x = 0; x < bmp.Width; x++)
                        {
                            var c = bmp.GetPixel(x, y);
                            if (c.G > 128)
                                fvs.Data[y * fvs.Width + x] = 0;
                            else if (c.R > 128)
                                fvs.Data[y * fvs.Width + x] = 1;
                            else if (c.B > 128)
                                fvs.Data[y * fvs.Width + x] = 2;
                        }
                    }
                    return fvs;
                }
            }


            for (int i = 0; i < 10; i++)
            {
                var vs = Load(@"D:\Users\NotImpLife\Projects\PPMLib\ATest\cat.png");
                var cmp = new FlipnoteVisualSourceComponent(ComponentsManager.BitmapProcessor, vs);
                cmp.Size = new Size(30 + i * 10, 30 + i * 10);
                cmp.BitmapChanged += (vc) =>
                {
                    Debug.WriteLine("Bitmap changed");
                    Invalidate();
                };
                cmp.Rotation = i * 10;
                cmp.UpdateTransform();

                AddComponent(cmp);
            }            
        }

        private void ComponentsManager_SelectionChanged(object sender, System.EventArgs e)
        {
            ControlPoints.Clear();
            switch(ComponentsManager.SelectionCount)
            {
                case 0: State.SetNormalMode(); break;
                case 1:
                    var selectedComponent = ComponentsManager.GetSelectedComponents().First();
                    State.SetSingleSelectMode(selectedComponent);
                    LoadControlPoints(selectedComponent);
                    break;
                default : State.SetMultiSelectMode(); break;
            }
            Invalidate();
        }

        public void AddComponent(VisualComponent component)
        {
            ComponentsManager.AddComponent(component);
            Invalidate();
        }

        private bool IsComponentInVisibleArea(VisualComponent component)
        {
            var r = SceneToClient(new Rectangle(component.Location, component.Size));            
            return ClientRectangle.IntersectsWith(r);           
        }

        #endregion

        #region UserState

        private struct _State
        {
            public int Mode;
            public VisualComponent SelectedComponent;

            public void SetNormalMode() => Mode = ModeNormal;
            public void SetSingleSelectMode(VisualComponent target)
            {
                Mode = ModeSingleSelect;
                SelectedComponent = target;
            }

            public void SetMultiSelectMode() => Mode = ModeMultiSelect;

            public const int ModeNormal = 0;
            public const int ModeSingleSelect = 1;
            public const int ModeMultiSelect = 2;
        }

        private _State State = new _State();

        #endregion UserState


        #region MouseGestures
        void InitializeMouseGestures()
        {
            MouseGestures.DragStart += MouseGestures_DragStart;
            MouseGestures.Drag += UserDataDragDropAutoManagement.MouseGestures_Drag;
            MouseGestures.Drop += UserDataDragDropAutoManagement.MouseGestures_Drop;
            MouseGestures.Zoom += MouseGestures_Zoom; ;
            MouseGestures.Click += MouseGestures_Click;

            MouseGestures.AttachTarget(this);
        }

        private void MouseGestures_Click(object sender, ClickGestureArgs e)
        {
            ComponentsManager.TriggerSelect(ClientToScene(e.Location), overwrite: (ModifierKeys & Keys.Control) != Keys.Control);
        }

        private void MouseGestures_Zoom(object sender, ZoomGestureArgs e)
        {
            var newScaleFactor = (pZoom + e.Factor).Clamp(10, 2000);
            var x = e.CursorLocation.X + (pViewOffset.X - e.CursorLocation.X) * newScaleFactor / pZoom;
            var y = e.CursorLocation.Y + (pViewOffset.Y - e.CursorLocation.Y) * newScaleFactor / pZoom;            
            pViewOffset = new Point(x, y);
            pZoom = newScaleFactor;
            Invalidate();
        }

        private void MouseGestures_DragStart(object sender, DragGestureArgs e)
        {
            //Debug.WriteLine($"Drag Start Mode={State.Mode}");
            if(HoveredControlPoint!=null)
            {
                Debug.WriteLine("HCP!!!!!!!!!!!!!!!");
                e.UserData = new ControlPointMoveGesture(HoveredControlPoint, ClientToScene(e.StartLocation));
                return;
            }

            if (State.Mode == _State.ModeSingleSelect || State.Mode == _State.ModeMultiSelect)
            {
                var comps = ComponentsManager.GetSelectedComponents().Select(c => (Component:c, OriginalLocation:c.Location)).ToArray();
                var point = ClientToScene(e.CurrentLocation);
                if (comps.Any(c => c.Component.HitTest(point))) 
                {
                    e.UserData = new SelectionMoveGesture(comps);
                    return;
                }                
            }

            e.UserData = new CanvasMoveGesture(ViewOffset);
        }
        #endregion MouseGestures

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.SmoothingMode = SmoothingMode.None;                        
            
            e.Graphics.TranslateTransform(pViewOffset.X, pViewOffset.Y);
            e.Graphics.ScaleTransform(pZoom * 0.01f, pZoom * 0.01f);
            e.Graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, 256, 192));

            var cState = e.Graphics.Save();

            e.Graphics.Transform = new Matrix();
            foreach (var c in ComponentsManager.GetComponents().Where(IsComponentInVisibleArea))
            {
                c.UpdateTransform();
                e.Graphics.DrawPolygon(ComponentOutlinePen, c.GetPolygonBounds().Select(SceneToClient).ToArray());
            }

            e.Graphics.Restore(cState);
            cState = e.Graphics.Save();

            foreach (var c in ComponentsManager.GetComponents().Where(IsComponentInVisibleArea)) 
            {                
                c.BitmapKey.UseBitmap(bmp =>
                {
                    if (bmp == null) return;
                    e.Graphics.DrawImage(bmp, c.Bounds);                    
                });               
            }

            e.Graphics.DrawRectangle(FlipnotePens.ThemeAccent, -0.5f, -0.5f, 257, 193);
            e.Graphics.Transform = new Matrix();

            foreach (var c in ComponentsManager.GetSelectedComponents().Where(IsComponentInVisibleArea)) 
            {
                e.Graphics.DrawPolygon(SelectionHighlightPen, c.GetPolygonBounds().Select(SceneToClient).ToArray());
            }

            foreach (var controlPoint in ControlPoints)
            {
                var pos = SceneToClient(controlPoint.GetRealPosition());
                e.Graphics.FillEllipse(Brushes.White, pos.X - 3, pos.Y - 3, 6, 6);
                e.Graphics.DrawEllipse(SelectionHighlightPen, pos.X - 3, pos.Y - 3, 6, 6);
            }
        }

        static int Distance(Point A, Point B)
        {
            var dx = A.X - B.X;
            var dy = A.Y - B.Y;
            return dx * dx + dy * dy;
        }        

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var S = ClientToScene(e.Location);
            HoveredControlPoint = ControlPoints.Where(c => Distance(S, c.GetRealPosition()) < 25).FirstOrDefault();            
            base.OnMouseMove(e);
        }

        #endregion Overrides

        #region Public Properties

        [Category("View Control")]        
        public Point ViewOffset
        {
            get => pViewOffset;
            set { pViewOffset = value; Invalidate(); }
        }

        [Category("View Control"), DefaultValue(100)]
        public int Zoom
        {
            get => pZoom;
            set { pZoom = value.Clamp(10, 2000); Invalidate(); }
        }
        #endregion Public Properties

        #region MouseGestureClasses

        class CanvasMoveGesture : IUserDataDragDrop
        {
            private Point StartPoint;            
            public CanvasMoveGesture(Point startPoint)
            {
                StartPoint = startPoint;
            }

            public void OnDrag(object sender, DragGestureArgs e)
            {
                var scene = sender as VisualComponentsScene;
                var offset = StartPoint;
                offset.Offset(e.DeltaLocation);
                scene.ViewOffset = offset;
            }

            public void OnDrop(object sender, DropGestureArgs e) { }
        }

        class SelectionMoveGesture : IUserDataDragDrop
        {

            private readonly (VisualComponent Component, Point OriginalLocation)[] Items;

            public SelectionMoveGesture((VisualComponent Component, Point OriginalLocation)[] items)
            {
                Items = items;
            }

            public void OnDrag(object sender, DragGestureArgs e)
            {
                var scene = sender as VisualComponentsScene;                                

                for(int i=0;i<Items.Length;i++)
                {
                    var l = Items[i].OriginalLocation;
                    l.Offset(e.DeltaLocation.X * 100 / scene.Zoom, e.DeltaLocation.Y * 100 / scene.Zoom);
                    Items[i].Component.Location = l;
                }
                scene.Invalidate();                
            }

            public void OnDrop(object sender, DropGestureArgs e) { }
        }

        class ControlPointMoveGesture : IUserDataDragDrop
        {

            ControlPoint Point;

            public ControlPointMoveGesture(ControlPoint point, Point sceneDragStartLocation)
            {
                Point = point;
                Point.OnMoveStarted(sceneDragStartLocation);
            }

            public void OnDrag(object sender, DragGestureArgs e)
            {                
                var scene = sender as VisualComponentsScene;
                Point.OnMoved(scene.ClientToScene(e.CurrentLocation));
                scene.Invalidate();                
            }

            public void OnDrop(object sender, DropGestureArgs e) { }
        }


        #endregion MouseGestureClasses

        #region Coordinates

        public Point SceneToClient(Point scenePoint)
        {
            var x = pViewOffset.X + Zoom * scenePoint.X / 100;
            var y = pViewOffset.Y + Zoom * scenePoint.Y / 100;
            return new Point(x, y);
        }
        public Point ClientToScene(Point clientPoint)
        {
            var x = (clientPoint.X - pViewOffset.X) * 100 / Zoom;
            var y = (clientPoint.Y - pViewOffset.Y) * 100 / Zoom;
            return new Point(x, y);
        }

        public Rectangle SceneToClient(Rectangle sceneRect)
        {
            var tl = new Point(sceneRect.Left, sceneRect.Top);
            var br = new Point(sceneRect.Right, sceneRect.Bottom);
            tl = SceneToClient(tl);
            br = SceneToClient(br);
            return new Rectangle(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
        }

        #endregion



    }
}
