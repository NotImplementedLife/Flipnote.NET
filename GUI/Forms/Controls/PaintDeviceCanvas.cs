using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Canvas;
using FlipnoteDotNet.GUI.Canvas.Components;
using FlipnoteDotNet.GUI.MouseGestures;
using FlipnoteDotNet.Utils.Paint;
using FlipnoteDotNet.Utils.Paint.Tools;
using PPMLib.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FlipnoteDotNet.GUI.Forms.Controls
{
    public class PaintDeviceCanvas : CanvasSpaceControl, IPaintDevice
    {
        public class TColorContext
        { 
            public int Color1 { get; set; }
            public int Color2 { get; set; }

            public int GetColor(int index)
            {
                switch (index)
                {                    
                    case 1: return Color1;
                    case 2:return Color2;
                    default: return 0;
                }
            }
        }

        class DrawingChunk
        {
            public int ChunkX { get; }
            public int ChunkY { get; }

            public bool IsDirty { get; set; }            

            public int[] Data { get; } = new int[Width * Height];

            public int GetPixel(int x, int y) => Data[Width * y + x];
            public void SetPixel(int x, int y, int value)
            {
                Data[Width * y + x] = value;
                IsDirty = true;
            }

            public DrawingChunk(int chunkX, int chunkY)
            {
                ChunkX = chunkX;
                ChunkY = chunkY;
                IsDirty = true;
            }

            public ICanvasComponent CurrentCanvasComponent { get; set; } = null;

            public ICanvasComponent ToCanvasComponent(TColorContext colorContext)
            {
                var bmp = Data.Select(colorContext.GetColor).ToArray().ToBitmap32bppPArgb(Width, Height);
                return new BitmapComponent(bmp, Width * ChunkX, Height * ChunkY)
                {
                    IsResizeable = false,
                    IsFixed = true                    
                };
            }

            public static readonly int Width = 64;
            public static readonly int Height = 64;
        }

        public Rectangle DrawingBounds = new Rectangle(0, 0, 512, 512);

        private Dictionary<Point, DrawingChunk> Chunks = new Dictionary<Point, DrawingChunk>();

        private DrawingChunk GetOrCreateChunk(int chkX, int chkY)
        {
            var key = new Point(chkX, chkY);
            return Chunks.TryGetValue(key, out var result) ? result : (Chunks[key] = new DrawingChunk(chkX, chkY));
        }

        public PreviewPoint CreatePreviewPoint(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DiscardPreview()
        {
            throw new NotImplementedException();
        }

        public int GetPixel(int x, int y)
        {
            if (x < DrawingBounds.Left || x >= DrawingBounds.Right || y < DrawingBounds.Top || y >= DrawingBounds.Bottom)
                return 0;

            int cx = (x >= 0 ? x : x - DrawingChunk.Width) / DrawingChunk.Width;
            int cy = (y >= 0 ? y : y - DrawingChunk.Height) / DrawingChunk.Height;
            var chunk = GetOrCreateChunk(cx, cy);
            x -= cx * DrawingChunk.Width;
            y -= cy * DrawingChunk.Height;
            return chunk.GetPixel(x, y);
        }

        public void PreviewLine(PreviewPoint p1, PreviewPoint p2)
        {
            throw new NotImplementedException();
        }

        public void PushPreview()
        {
            throw new NotImplementedException();
        }

        public void SetPixel(int x, int y, int pixel)
        {
            if (x < DrawingBounds.Left || x >= DrawingBounds.Right || y < DrawingBounds.Top || y >= DrawingBounds.Bottom)
                return;
            int cx = (x >= 0 ? x : x - DrawingChunk.Width + 1) / DrawingChunk.Width;
            int cy = (y >= 0 ? y : y - DrawingChunk.Height + 1) / DrawingChunk.Height;
            var chunk = GetOrCreateChunk(cx, cy);
            x -= cx * DrawingChunk.Width;
            y -= cy * DrawingChunk.Height;            

            chunk.SetPixel(x, y, pixel);
        }

        public void StartPreview()
        {
            throw new NotImplementedException();
        }

        ICanvasComponent BoundsComponent;

        public TColorContext ColorContext = new TColorContext();

        public void SetColors(Color color1, Color color2)
        {
            ColorContext.Color1 = color1.ToArgb();
            ColorContext.Color2 = color2.ToArgb();
            ForceUpdate();
        }

        public void UpdateDevice()
        {
            foreach(var chunk in Chunks.Values)            
                if (chunk.IsDirty) 
                {
                    RemoveComponent(chunk.CurrentCanvasComponent);
                    chunk.CurrentCanvasComponent = chunk.ToCanvasComponent(ColorContext);
                    AddComponent(chunk.CurrentCanvasComponent);
                    chunk.IsDirty = false;
                }
            RemoveComponent(BoundsComponent);
            AddComponent(BoundsComponent = new SimpleRectangle(DrawingBounds) { IsFixed = true, Pen = Pens.Red });
            Invalidate();
        }

        MouseGesturesHandler DrawingOperationsHandler = new MouseGesturesHandler();

        IPaintOperation CurrentPaintOperation = null;

        public void AttachOperation(IPaintOperation operation)
        {
            if (operation == CurrentPaintOperation) return;

            if(CurrentPaintOperation != null)
            {
                CurrentPaintOperation.Device = null;
            }

            CurrentPaintOperation = operation;
            if (CurrentPaintOperation == null)
            {
                DrawingOperationsHandler.DetachTarget();
                EnableMouseGestures();
                return;
            }
            CurrentPaintOperation.Device = this;
            DisableMouseGestures();                  
            DrawingOperationsHandler.AttachTarget(this);
        }

        public PaintDeviceCanvas()
        {
            ColorContext.Color1 = Constants.Colors.FlipnoteBlue.ToArgb();
            ColorContext.Color2 = Constants.Colors.FlipnoteRed.ToArgb();

            DrawingOperationsHandler.Click += DrawingOperationsHandler_Click;
            DrawingOperationsHandler.DragStart += DrawingOperationsHandler_DragStart;
            DrawingOperationsHandler.Drag += DrawingOperationsHandler_Drag;
            DrawingOperationsHandler.Drop += DrawingOperationsHandler_Drop;
        }        

        private void DrawingOperationsHandler_Click(object sender, ClickGestureArgs e)
        {
            CurrentPaintOperation?.OnClick(this, new ClickGestureArgs(ScreenToCanvas(e.Location)));
        }

        private void DrawingOperationsHandler_DragStart(object sender, DragGestureArgs e)
        {
            var e2 = new DragGestureArgs(ScreenToCanvas(e.StartLocation), ScreenToCanvas(e.CurrentLocation));
            CurrentPaintOperation?.OnDragStart(this, e2);
            e.UserData = e2.UserData;
            if (e2.IsCanceled) e.Cancel();
        }

        private void DrawingOperationsHandler_Drag(object sender, DragGestureArgs e)
        {
            var e2 = new DragGestureArgs(ScreenToCanvas(e.StartLocation), ScreenToCanvas(e.CurrentLocation));
            e2.UserData = e.UserData;
            CurrentPaintOperation?.OnDrag(this, e2);
            e.UserData = e2.UserData;
            if (e2.IsCanceled) e.Cancel();
        }

        private void DrawingOperationsHandler_Drop(object sender, DropGestureArgs e)
        {
            var e2 = new DropGestureArgs(ScreenToCanvas(e.StartLocation), ScreenToCanvas(e.CurrentLocation), e.UserData);
            CurrentPaintOperation?.OnDrop(this, e2);            
        }

        public void ForceUpdate()
        {
            ClearComponents();
            foreach (var chunk in Chunks.Values)
            {
                chunk.CurrentCanvasComponent = chunk.ToCanvasComponent(ColorContext);
                AddComponent(chunk.CurrentCanvasComponent);
                chunk.IsDirty = false;
            }
            AddComponent(BoundsComponent = new SimpleRectangle(DrawingBounds) { IsFixed = true, Pen = Pens.Red });
            Invalidate();

        }

        public void LoadFromFlipnoteVisualSoruce(FlipnoteVisualSource source)
        {
            for(int y=0;y< source.Height;y++)
            {
                for (int x = 0; x < source.Width; x++)
                {
                    int pixel = source.Data[y * source.Width + x];
                    if (pixel != 0)
                    {
                        SetPixel(x, y, pixel);
                    }
                }
            }
            ForceUpdate();
        }

        public FlipnoteVisualSource ToFlipnoteVisualSource()
        {
            var pixels = new List<(int X, int Y, int V)>();
            foreach (var chunk in Chunks.Values)
            {
                int cx = chunk.ChunkX * DrawingChunk.Width;
                int cy = chunk.ChunkY * DrawingChunk.Height;
                for (int y = 0; y < DrawingChunk.Height; y++) 
                {
                    for (int x = 0; x < DrawingChunk.Width; x++)
                    {
                        if (chunk.GetPixel(x, y) == 1)
                            pixels.Add((cx + x, cy + y, 1));
                        else if (chunk.GetPixel(x, y) == 2)
                            pixels.Add((cx + x, cy + y, 2));
                    }
                }
            }

            if (pixels.Count == 0)
                return new FlipnoteVisualSource(8, 8);

            var xmin = pixels.Min(_ => _.X);
            var ymin = pixels.Min(_ => _.Y);

            pixels = pixels.Select(_ => (X: _.X - xmin, Y: _.Y - ymin, V: _.V)).ToList();

            var width = pixels.Max(_ => _.X) + 1;
            var height = pixels.Max(_ => _.Y) + 1;

            var source = new FlipnoteVisualSource(width, height);
            pixels.ForEach(_ => source.Data[_.Y * width + _.X] = (byte)_.V);
            return source;
        }
    }
}
