using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Controls;
using FlipnoteDotNet.GUI.MouseGestures;
using FlipnoteDotNet.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;

namespace FlipnoteDotNet.GUI.Tracks
{
    [ToolboxItem(true)]
    internal class SequenceTracksViewer : ScrollView
    {
        MouseGesturesHandler MouseGesturesHandler = new MouseGesturesHandler();

        private SequenceManager _SequenceManager= new SequenceManager();

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public SequenceManager SequenceManager
        {
            get => _SequenceManager;
            set
            {
                _SequenceManager = value;
                AdjustSurfaceSize();
                Invalidate();
            }
        }

        public SequenceTracksViewer()
        {
            InitializeComponent();

            MouseGesturesHandler.DragStart += MouseGesturesHandler_DragStart;
            MouseGesturesHandler.Drag += MouseGesturesHandler_Drag;
            MouseGesturesHandler.Drop += MouseGesturesHandler_Drop;
            MouseGesturesHandler.Zoom += MouseGesturesHandler_Zoom;
            MouseGesturesHandler.Click += MouseGesturesHandler_Click;
            MouseGesturesHandler.AttachTarget(this.ScrollContainer);

            SequenceManager.ElementAdded += SequenceManager_ElementAdded;
            SequenceManager.ElementRemoved += SequenceManager_ElementRemoved;

            ScrollContainer.MouseMove += ScrollContainer_MouseMove;
        }

        private IEnumerable<(Sequence Element, Rectangle Bounds, int TrackId)> GetVisibleElements()
        {
            var rect = TracksPanelBounds;
            int y = rect.Top - ScrollY;            
            for (int i = 0; i < SequenceManager.TracksCount; i++, y += TrackHeight)
            {
                if (y < rect.Top - TrackHeight) 
                    continue;
                var track = SequenceManager.GetTrack(i);
                foreach (var elem in track.GetSequences()) 
                {
                    var x1 = TrackToScreen(elem.StartFrame);
                    var x2 = TrackToScreen(elem.EndFrame);
                    if (x2 < rect.Left || x1 >= rect.Right) continue;
                    var r = new Rectangle(x1, y + TrackPadding, Math.Max(1, x2 - x1), TrackHeight - 2 * TrackPadding);
                    yield return (elem, r, i);
                }
            }
        }

        private void ScrollContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if(IsSequenceCreateMode)
            {
                using (var ms = new MemoryStream(Resources.cur_cross))                
                    Cursor = new Cursor(ms);
                return;
            }            

            var x1 = e.Location.X - 3;
            var x2 = e.Location.X + 3;
            var y = e.Location.Y;
            Cursor = Cursors.Default;
            foreach (var (_, bounds, _) in GetVisibleElements()) 
                if (y.IsInRange(bounds.Top, bounds.Bottom) && (bounds.Right.IsInRange(x1, x2) || bounds.Left.IsInRange(x1, x2))) 
                {
                    Cursor = Cursors.SizeWE;
                    break;
                }            
        }

        private bool ElementRemovedEventActive = true;

        private void SequenceManager_ElementRemoved(SequenceManager sender, SequenceTrack track, Sequence e)
        {
            if (!ElementRemovedEventActive) return;
            if(e==SelectedElement)
            {
                _SelectedElement = null;
                SelectedElementChanged?.Invoke(this, new EventArgs());
            }

            InvalidateSurface();
        }

        private void SequenceManager_ElementAdded(SequenceManager sender, SequenceTrack track, Sequence e)
        {
            InvalidateSurface();
        }

        private void MouseGesturesHandler_Click(object sender, ClickGestureArgs e)
        {
            var oldSelection = _SelectedElement;
            _SelectedElement = null;
            foreach (var (elem, bounds, _) in GetVisibleElements()) 
            {
                if (bounds.Contains(e.Location))
                {
                    _SelectedElement = elem;
                    break;
                }
            }             
            InvalidateSurface();

            if (oldSelection != _SelectedElement) 
            {
                SelectedElementChanged?.Invoke(this, new EventArgs());
            }
        }

        private bool IsSequenceCreateMode = false;
        private Rectangle SequenceCreatePreviewBounds = new Rectangle();

        public void StartSequenceCreateMode()
        {
            IsSequenceCreateMode = true;
        }

        public void EndSequenceCreateMode()
        {
            IsSequenceCreateMode = false;
            SequenceCreateModeEnded?.Invoke(this, new EventArgs());
        }

        public event EventHandler SequenceCreateModeEnded;

        private void MouseGesturesHandler_DragStart(object sender, DragGestureArgs e)
        {            
            if(IsSequenceCreateMode)
            {
                var trackLine = ScreenToTrackLineId(e.StartLocation.Y);
                if (!trackLine.IsInRange(0, SequenceManager.TracksCount)) 
                {
                    EndSequenceCreateMode();
                    e.Cancel();
                    return;
                }

                e.UserData = new SequenceCreateDragData(ScreenToTrack(e.StartLocation.X), trackLine);
                return;
            }

            if (TrackbarPanelBounds.Contains(e.StartLocation))
            {
                e.UserData = new TrackSignMoveDragData();
                return;
            }
            var x1 = e.StartLocation.X - 3;
            var x2 = e.StartLocation.X + 3;
            var y = e.StartLocation.Y;
            Cursor = Cursors.Default;
            foreach (var (elem, bounds, _) in GetVisibleElements()) 
            {
                if (y.IsInRange(bounds.Top, bounds.Bottom))
                {
                    if(bounds.Left.IsInRange(x1, x2))
                    {
                        e.UserData = new SequenceResizeDragData(elem, SequenceResizeDragData._Direction.Left);
                        return;

                    }
                    if (bounds.Right.IsInRange(x1, x2)) 
                    {
                        e.UserData = new SequenceResizeDragData(elem, SequenceResizeDragData._Direction.Right);                        
                        return;
                    }                                      
                    if(bounds.Contains(e.StartLocation))
                    {
                        e.UserData = new SequenceMoveDragData(elem);
                        SelectElement(elem);
                        return;
                    }
                }
            }
        }

        private void SelectElement(Sequence e)
        {
            if (SelectedElement == e) return;
            _SelectedElement = e;
            SelectedElementChanged?.Invoke(this, new EventArgs());
        }        

        private void MouseGesturesHandler_Drag(object sender, DragGestureArgs e)
        {            
            if (e.UserData is TrackSignMoveDragData) 
            {
                TrackSignPosition = ScreenToTrackSignPosition(e.CurrentLocation.X);
                CurrentFrameChanged?.Invoke(this, new EventArgs());
                return;
            }
            if(e.UserData is SequenceResizeDragData resizeData)
            {                
                resizeData.Resize(e.DeltaLocation.X * 100 / Zoom);
                InvalidateSurface();
                return;
            }
            if(e.UserData is SequenceMoveDragData moveData)
            {
                var elem = moveData.Element;
                int oldS = elem.StartFrame;
                int oldE = elem.EndFrame;
                moveData.Move(e.DeltaLocation.X * 100 / Zoom);                
                
                int trackId = ScreenToTrackLineId(e.CurrentLocation.Y).Clamp(0, SequenceManager.TracksCount - 1);                

                bool trackChanged = SequenceManager.GetTrack(trackId) != elem.Track;
                                
                var newR = GetScreenRectangleOnTrack(trackId, elem.StartFrame, elem.EndFrame);

                var crsOverlappedElem = GetVisibleElements()
                    .Where(_ => _.Element != elem && (_.TrackId == trackId || SequenceManager.GetTrack(_.TrackId) == elem.Track) && _.Bounds.IntersectsWith(newR))
                    .FirstOrDefault();
                bool crsOverlapsElems = crsOverlappedElem.Element != null;

                if (!crsOverlapsElems)
                {
                    ElementRemovedEventActive = false;
                    elem.Track.RemoveSequence(elem);
                    SequenceManager.GetTrack(trackId).AddSequence(elem);
                    ElementRemovedEventActive = true;
                }
                else if (!trackChanged)
                {                
                    elem.StartFrame = oldS;
                    elem.EndFrame = oldE;
                }                
                else
                {
                    var q = GetVisibleElements()
                        .Where(_ => _.Element != elem && SequenceManager.GetTrack(_.TrackId) == elem.Track)
                        .FirstOrDefault();
                    if(q.Element!=null)
                    {
                        newR = GetScreenRectangleOnTrack(q.TrackId, elem.StartFrame, elem.EndFrame);
                        if(q.Bounds.IntersectsWith(newR))
                        {
                            elem.StartFrame = oldS;
                            elem.EndFrame = oldE;
                        }
                    }                
                }
                InvalidateSurface();
                return;
            }

            if (e.UserData is SequenceCreateDragData createData)
            {
                var x1 = createData.StartX;
                var t = createData.TrackId;
                var x2 = createData.EndX = ScreenToTrack(e.CurrentLocation.X);
                var tb = TrackToScreenLimits(t);
                SequenceCreatePreviewBounds = new Rectangle(x1, tb.Top, x2 - x1, tb.Bottom - tb.Top);
                InvalidateSurface();
                return;
            }

        }
        private void MouseGesturesHandler_Drop(object sender, DropGestureArgs e)
        {
            if(e.UserData is TrackSignMoveDragData)
            {                
                return;
            }
            if(e.UserData is SequenceCreateDragData createData)
            {
                var startX = createData.StartX;
                var endX = createData.EndX;
                var trackId = createData.TrackId;
                var newR = GetScreenRectangleOnTrack(trackId, startX, endX);
                bool overlapsElems = GetVisibleElements().Where(_ => _.TrackId == trackId && _.Bounds.IntersectsWith(newR)).Any();
                if(!overlapsElems)
                {
                    SequenceManager.GetTrack(trackId).AddSequence(new Sequence(startX, endX));
                }

                EndSequenceCreateMode();
                InvalidateSurface();
                return;
            }
        }

        public event EventHandler CurrentFrameChanged;

        private void MouseGesturesHandler_Zoom(object sender, ZoomGestureArgs e)
        {
            Zoom += e.Factor * (Zoom < 400 ? 1 : Zoom / 16);            
            Invalidate();
        }

        public void InitializeComponent()
        {
            SurfacePaint += SequenceTracksViewer_SurfacePaint;            
            Zoom = 100;
        }

        public int TrackSignScreenX => LeftPanelWidth - ScrollX + TrackSignPosition * Zoom / 100;

        private int ScreenToTrackSignPosition(int value)
        {
            return (value - LeftPanelWidth + ScrollX) * 100 / Zoom;
        }

        private int _Zoom = 100;

        [DefaultValue(100)]        
        public int Zoom
        {
            get => _Zoom;
            set
            {
                _Zoom = value.Clamp(40, ThumbnailReservedWidth * 100);
                AdjustSurfaceSize();
                InvalidateSurface();                
            }
        }


        private int _TrackSignPosition = 0;
        public int TrackSignPosition
        {
            get => _TrackSignPosition;
            set
            {
                _TrackSignPosition = value.Clamp(0, 999);
                InvalidateSurface();
            }
        }

        private Font BottomBarFont = new Font("Calibri", 8);
        private Size ThumbnailSize = new Size(60, 45);
        private int BottomBarPadding = 3;
        private int ThumbnailMargin = 3;
        private int ThumbnailReservedWidth => ThumbnailSize.Width + 2 * ThumbnailMargin;
        private int BottomBarHeight => ThumbnailSize.Height + BottomBarFont.Height + 2 * BottomBarPadding;
        private int LeftPanelWidth = 100;
        private int TrackBarPanelHeight = 25;

        private int TrackHeight = 50;
        private int TrackPadding = 3;

        Rectangle TrackbarPanelBounds => new Rectangle(0, 0, ContainerSize.Width, TrackBarPanelHeight);
        Rectangle TracksPanelBounds => new Rectangle(LeftPanelWidth, TrackBarPanelHeight, ContainerSize.Width - LeftPanelWidth, ContainerSize.Height - BottomBarHeight - TrackBarPanelHeight);

        private void SequenceTracksViewer_SurfacePaint(object sender, PaintEventArgs e)
        {
            var gw = ContainerSize.Width;// (int)e.Graphics.ClipBounds.Width;
            var gh = ContainerSize.Height;// (int)e.Graphics.ClipBounds.Height;
            var bottomBarY = gh - BottomBarHeight;

            ClipDraw(DrawTracksPanel, e.Graphics, TracksPanelBounds);
            ClipDraw(DrawLeftPanel, e.Graphics, new Rectangle(0, TrackBarPanelHeight, LeftPanelWidth, gh));
            ClipDraw(DrawBottomBar, e.Graphics, new Rectangle(LeftPanelWidth, bottomBarY, gw - LeftPanelWidth, BottomBarHeight));
            ClipDraw(DrawTrackbarPanel, e.Graphics, TrackbarPanelBounds);
            ClipDraw(DrawTrackLabels, e.Graphics, new Rectangle(0, TrackBarPanelHeight, LeftPanelWidth, bottomBarY - TrackBarPanelHeight));
        }

        private Sequence _SelectedElement = null;
        public Sequence SelectedElement => _SelectedElement;

        public event EventHandler SelectedElementChanged;

        private void DrawTracksPanel(Graphics g, Rectangle rect)
        {
            g.FillRectangle(Brushes.White, rect);

            int y = rect.Top - ScrollY;            

            for (int i = 0; i < SequenceManager.TracksCount; i++, y += TrackHeight)
            {
                if (y < rect.Top - TrackHeight)
                    continue;
                g.FillRectangle(Colors.FlipnoteThemeSecondaryColor.GetBrush(), rect.Left, y + TrackPadding, rect.Right, TrackHeight - 2 * TrackPadding);
                g.DrawLine(Colors.FlipnoteThemeAccentColor.GetPen(2), rect.Left, y + TrackPadding, rect.Right, y + TrackPadding);
                g.DrawLine(Colors.FlipnoteThemeAccentColor.GetPen(2), rect.Left, y + TrackPadding + TrackHeight - 2 * TrackPadding, rect.Right, y + TrackPadding + TrackHeight - 2 * TrackPadding);
            }

            int trackSignX = TrackSignScreenX;
            g.FillRectangle(Colors.FlipnoteThemeAccentColor.Alpha(64).GetBrush(), trackSignX, rect.Top, Zoom / 100, rect.Width);

            foreach (var (elem, r, _) in GetVisibleElements()) 
            {                
                var color2 = elem == _SelectedElement ? Color.Yellow : Color.White;

                var brush = new LinearGradientBrush(r, elem.Color, color2, 90f);
                var blend = new Blend();
                blend.Positions = new float[] { 0.0f, 0.8f, 1.0f };
                blend.Factors = new float[] { 0.0f, 0.2f, 1.0f };
                brush.Blend = blend;
                g.FillRoundedRectangle(brush, r, 5);
                g.DrawRoundedRectangle(elem.Color.GetPen(1), r, 5);

                var textR = new Rectangle(r.X + 5, r.Y + 5, r.Width - 10, r.Height - 10);
                var format = new StringFormat(StringFormatFlags.NoWrap);
                g.DrawString(elem.Name, Font, color2.GetBrush(), textR, format);
            }            
            g.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(2), rect.Left, rect.Bottom, rect.Right, rect.Bottom);            
            g.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(3), trackSignX, rect.Top, trackSignX, rect.Bottom);


            if(IsSequenceCreateMode)
            {
                var sx1 = TrackToScreen(SequenceCreatePreviewBounds.Left);
                var sx2 = TrackToScreen(SequenceCreatePreviewBounds.Right);
                var sy = SequenceCreatePreviewBounds.Top;
                var sh = SequenceCreatePreviewBounds.Height;
                g.DrawRectangle(Pens.Black, sx1, sy, sx2 - sx1, sh);
            }            
        }

        private void DrawTrackLabels(Graphics g, Rectangle rect)
        {
            int x = rect.Left + TrackPadding;
            int y = rect.Top - ScrollY;
            int w = rect.Width - 2 * TrackPadding;
            int h = TrackHeight - 2 * TrackPadding;

            for (int i=0;i<SequenceManager.TracksCount;i++)
            {
                g.DrawRectangle(Colors.FlipnoteThemeAccentColor.GetPen(2), x, y + TrackPadding, w + TrackPadding, h);

                string label = $"Track {i + 1}";
                var labelSize = g.MeasureString(label, Font);
                g.DrawString(label, Font, Colors.FlipnoteThemeAccentColor.GetBrush(),
                    rect.Width - labelSize.Width - 2 * TrackPadding, y + (TrackHeight - labelSize.Height) / 2);
                y += TrackHeight;
            }
            g.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(2), rect.Left, rect.Bottom, rect.Right, rect.Bottom);
        }

        private void DrawTrackbarPanel(Graphics g, Rectangle rect)
        {            
            g.FillRectangle(Colors.FlipnoteThemeSecondaryColor.GetBrush(), rect);
            g.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(2), rect.Left, rect.Bottom, rect.Right, rect.Bottom);

            int trackSignX = TrackSignScreenX;
            if (trackSignX < LeftPanelWidth) return;

            int diam = rect.Height / 2;
            int cx = trackSignX - diam / 2;
            int cy = rect.Top + diam / 2;
            g.FillEllipse(Colors.FlipnoteThemeMainColor.GetBrush(), cx, cy, diam, diam);
            g.FillPolygon(Colors.FlipnoteThemeMainColor.GetBrush(), new Point[]
            {
                new Point(cx, cy+diam/2),new Point(cx+diam, cy+diam/2), new Point(cx+diam/2,rect.Bottom+3)
            });
            g.FillEllipse(Brushes.White, cx + 3, cy + 3, diam - 6, diam - 6);
        }

        private void DrawLeftPanel(Graphics g, Rectangle rect)
        {            
            var brush = new LinearGradientBrush(new Point(0, 0), new Point(100, 0), Color.White, Colors.FlipnoteThemeSecondaryColor);            
            g.FillRectangle(brush, rect);
            g.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(2), rect.Right, rect.Top, rect.Right, rect.Bottom);
        }

        private void DrawBottomBar(Graphics g, Rectangle rect)
        {            
            g.FillRectangle(Colors.FlipnoteThemeSecondaryColor.GetBrush(), rect);            

            var thumbnailsCount = 10 * Zoom / ThumbnailReservedWidth;
            var tx = rect.Left - (ScrollX % ThumbnailReservedWidth);

            for (int i = ScrollX / ThumbnailReservedWidth; i < thumbnailsCount && tx < Width; i++)
            {
                var thumbnailRect = new Rectangle(tx + ThumbnailMargin, rect.Top + BottomBarPadding, ThumbnailSize.Width, ThumbnailSize.Height);
                g.DrawRectangle(Colors.FlipnoteThemeMainColor.GetPen(), thumbnailRect);
                
                var text = $"{ScreenToTrackSignPosition(tx + ThumbnailSize.Width) + 1}";
                var textSize = g.MeasureString(text, BottomBarFont);

                var txtX = tx + ThumbnailMargin + (ThumbnailSize.Width - textSize.Width) / 2;
                var txtY = rect.Top + BottomBarPadding + ThumbnailSize.Height + 2;

                g.DrawString(text, BottomBarFont, Brushes.Black, txtX, txtY);
                tx += ThumbnailReservedWidth;
            }            
        }

        public void AdjustSurfaceSize()
        {
            for (int i = 0; i < 2; i++)
                SurfaceSize = new Size(
                    LeftPanelWidth + 1000 * Zoom / 100,
                    TrackHeight * SequenceManager.TracksCount + TrackBarPanelHeight + BottomBarHeight);
        }

        private void ClipDraw(Action<Graphics, Rectangle> action, Graphics g, Rectangle rect)
        {
            var bounds = g.ClipBounds;
            g.Clip = new Region(rect);
            action(g, rect);
            g.Clip = new Region(bounds);
        }

        private int ScreenToTrack(int x)
        {
            return (x - LeftPanelWidth + ScrollX) * 100 / Zoom;
        }

        private int TrackToScreen(int x)
        {
            return LeftPanelWidth - ScrollX + x * Zoom / 100;
        }

        private int ScreenToTrackLineId(int y)
        {
            int l = (y - TracksPanelBounds.Top + ScrollY);
            return l < 0 ? -1 : l / TrackHeight;
        }

        private (int Top, int Bottom) TrackToScreenLimits(int index)
        {
            var t = index * TrackHeight - ScrollY + TracksPanelBounds.Top;
            var b = t + TrackHeight;
            return (t, b);
        }

        private Rectangle GetScreenRectangleOnTrack(int trackId, int startTX, int endTX)
        {
            var tb = TrackToScreenLimits(trackId);
            var x1 = TrackToScreen(startTX);
            var x2 = TrackToScreen(endTX);
            return new Rectangle(x1, tb.Top, x2 - x1, tb.Bottom - tb.Top);
        }
    }
}
