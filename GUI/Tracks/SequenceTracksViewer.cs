using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Controls;
using FlipnoteDotNet.GUI.MouseGestures;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            MouseGesturesHandler.AttachTarget(this.ScrollContainer);
        }        

        private void MouseGesturesHandler_DragStart(object sender, DragGestureArgs e)
        {            
            if (TrackbarPanelBounds.Contains(e.StartLocation))
            {
                int trackSignX = TrackSignScreenX;
                int r = TrackbarPanelBounds.Height / 4;
                e.UserData = new TrackSignMoveDragData(trackSignX);

                /*if (e.StartLocation.X.IsInRange(trackSignX-r, trackSignX+r))
                {
                    
                }*/
            }
        }

        private void MouseGesturesHandler_Drag(object sender, DragGestureArgs e)
        {
            if (e.UserData is TrackSignMoveDragData trackData) 
            {
                TrackSignPosition = ScreenToTrackSignPosition(e.CurrentLocation.X);
            }

        }
        private void MouseGesturesHandler_Drop(object sender, DropGestureArgs e)
        {
        }

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

        private void SequenceTracksViewer_SurfacePaint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            var gw = ContainerSize.Width;// (int)e.Graphics.ClipBounds.Width;
            var gh = ContainerSize.Height;// (int)e.Graphics.ClipBounds.Height;
            var bottomBarY = gh - BottomBarHeight;

            ClipDraw(DrawTracksPanel, e.Graphics, new Rectangle(LeftPanelWidth, TrackBarPanelHeight, gw - LeftPanelWidth, bottomBarY - TrackBarPanelHeight));
            ClipDraw(DrawLeftPanel, e.Graphics, new Rectangle(0, TrackBarPanelHeight, LeftPanelWidth, gh));
            ClipDraw(DrawBottomBar, e.Graphics, new Rectangle(LeftPanelWidth, bottomBarY, gw - LeftPanelWidth, BottomBarHeight));
            ClipDraw(DrawTrackbarPanel, e.Graphics, TrackbarPanelBounds);
            ClipDraw(DrawTrackLabels, e.Graphics, new Rectangle(0, TrackBarPanelHeight, LeftPanelWidth, bottomBarY - TrackBarPanelHeight));
        }        

        private void DrawTracksPanel(Graphics g, Rectangle rect)
        {
            g.FillRectangle(Brushes.White, rect);

            int y = rect.Top - ScrollY;

            for (int i = 0; i < SequenceManager.Tracks.Count; i++, y += TrackHeight) 
            {
                if (y < rect.Top - TrackHeight)
                    continue;
                g.FillRectangle(Colors.FlipnoteThemeSecondaryColor.GetBrush(), rect.Left, y + TrackPadding, rect.Right, TrackHeight - 2 * TrackPadding);
                //g.DrawString($"{i}", Font, Brushes.Black, rect.Left, y + TrackPadding);
                g.DrawLine(Colors.FlipnoteThemeAccentColor.GetPen(2), rect.Left, y + TrackPadding, rect.Right, y + TrackPadding);
                g.DrawLine(Colors.FlipnoteThemeAccentColor.GetPen(2), rect.Left, y + TrackPadding + TrackHeight - 2 * TrackPadding, rect.Right, y + TrackPadding + TrackHeight - 2 * TrackPadding);
                
            }
            g.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(2), rect.Left, rect.Bottom, rect.Right, rect.Bottom);


            int trackSignX = TrackSignScreenX;
            g.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(3), trackSignX, rect.Top, trackSignX, rect.Bottom);
        }

        private void DrawTrackLabels(Graphics g, Rectangle rect)
        {
            int x = rect.Left + TrackPadding;
            int y = rect.Top - ScrollY;
            int w = rect.Width - 2 * TrackPadding;
            int h = TrackHeight - 2 * TrackPadding;

            for (int i=0;i<SequenceManager.Tracks.Count;i++)
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

                //var text = $"{i * 1000 / thumbnailsCount + 1}";
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
                    TrackHeight * SequenceManager.Tracks.Count + TrackBarPanelHeight + BottomBarHeight);
        }

        private void ClipDraw(Action<Graphics, Rectangle> action, Graphics g, Rectangle rect)
        {
            var bounds = g.ClipBounds;
            g.Clip = new Region(rect);
            action(g, rect);
            g.Clip = new Region(bounds);
        }
    }
}
