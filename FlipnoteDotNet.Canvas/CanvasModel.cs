using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using FlipnoteDotNet.Canvas.Components;

namespace FlipnoteDotNet.Canvas
{
    public class CanvasModel : IDisposable
    {
        private Bitmap FrontBuffer;
        private Bitmap BackBuffer;
        private Graphics FrontGraphics;
        private Graphics BackGraphics;
        private Color BackgroundColor = Color.White;

        public readonly int Width;
        public readonly int Height;

        private BindingList<CanvasComponent> Components;

        internal bool IsDirty = true;
        internal CanvasProcessor.DirtyLock DirtyLock;

        private CanvasProcessor Processor;

        public CanvasModel(int width, int height)
        {
            Width = width;
            Height = height;
            FrontBuffer = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);
            BackBuffer = new Bitmap(Width, Height, PixelFormat.Format32bppPArgb);            

            FrontGraphics = CreateGraphics(FrontBuffer);
            BackGraphics = CreateGraphics(BackBuffer);

            Components = new BindingList<CanvasComponent>(new List<CanvasComponent>());
            Components.ListChanged += Components_ListChanged;

            Processor = new CanvasProcessor(this);
            Processor.FrameUpdated += Processor_FrameUpdated;            
        }

        public void Clear()
        {
            Components.Clear();
        }

        public void BindComponentsList(IList<CanvasComponent> components)
        {            
            lock (ComponentsAccessLock)
            {
                for (int i = 0; i < Components.Count; i++)
                {
                    Components[i].DirtyLock = null;
                }
                Components.ListChanged -= Components_ListChanged;
                Components = new BindingList<CanvasComponent>(components);
                Components.ListChanged += Components_ListChanged;

                for (int i = 0; i < components.Count; i++)
                {
                    Components[i].DirtyLock = DirtyLock;
                }
            }
            DirtyLock.NotifyDirty();
            BindingListChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler BindingListChanged;

        public void StartProcessor() => Processor.Start();

        private void Processor_FrameUpdated(object sender, EventArgs e)
        {
            Bitmap capture;
            lock (BufferLock)
                capture = new Bitmap(FrontBuffer);               
            var ev = new CanvasFrameUpdatedEventArgs(capture);
            FrameUpdated?.Invoke(this, ev);
            if (ev.DisposeBitmap)
                capture.Dispose();          
        }

        public void DrawOnGraphics(Graphics g, int x, int y, int width, int height)
        {
            Bitmap capture;
            lock (BufferLock)
                capture = new Bitmap(FrontBuffer);
            g.DrawImage(capture, x, y, width, height);
        }

        public delegate void OnFrameUpdated(object sender, CanvasFrameUpdatedEventArgs e);
        public event OnFrameUpdated FrameUpdated;

        private void Components_ListChanged(object sender, ListChangedEventArgs e)
        {
            Debug.WriteLine("ListChanged");            
            DirtyLock.NotifyDirty();
        }

        private object ComponentsAccessLock = new object();
        private object BufferLock = new object();

        private void SwapBuffers()
        {
            (FrontBuffer, BackBuffer) = (BackBuffer, FrontBuffer);
            (FrontGraphics, BackGraphics) = (BackGraphics, FrontGraphics);
        }

        private void Render(Graphics g, CanvasComponent[] components)
        {            
            for (int i = 0, len = components.Length; i < len; i++)
            {
                components[i].Update();
                var state = g.Save();                
                if (components[i].IgnoreGraphicsTransform) 
                {                    
                    components[i].Draw(g);
                }
                else
                {
                    using (var transform = components[i].CloneDirectTransform())                                            
                        g.MultiplyTransform(transform);                    
                    components[i].Draw(g);                    
                }
                g.Restore(state);
            }
        }

        public void Update(bool force = false)
        {
            if (!force && !IsDirty) return;
            IsDirty = false;
            CanvasComponent[] components;
            lock (ComponentsAccessLock)
                components = Components.ToArray();
            BackGraphics.Clear(BackgroundColor);
            Render(BackGraphics, components);
            BackGraphics.Flush(FlushIntention.Flush);
            lock (BufferLock)
                SwapBuffers();            
        }        

        public void AddComponent(CanvasComponent component)
        {
            lock (ComponentsAccessLock)
            {
                Processor.AttachComponent(component);
                Components.Add(component);
            }
        }

        public void InsertComponent(int index, CanvasComponent component)
        {
            lock (ComponentsAccessLock)
            {
                Processor.AttachComponent(component);
                Components.Insert(index, component);
            }
        }
        
        public bool RemoveComponent(CanvasComponent component) 
        {
            lock (ComponentsAccessLock)
            {
                bool removed = Components.Remove(component);
                if (removed) Processor.DetachComponent(component);
                return removed;                
            }
        }                
        
        public void UpdateComponent(int index, CanvasComponent component)
        {
            lock (ComponentsAccessLock)
            {
                Processor.DetachComponent(Components[index]);
                Processor.AttachComponent(component);
                Components[index] = component;
            }
        }

        public CanvasComponent GetComponentAtLocation(Point p) => GetComponentAtLocation(p.X, p.Y);

        public CanvasComponent GetComponentAtLocation(int x, int y)
        {
            lock (ComponentsAccessLock)
            {
                int len = Components.Count;
                for (int i = len - 1; i >= 0; i--) 
                {
                    if (Components[i].HitTest(x, y, out _, out _))
                        return Components[i];
                }
            }
            return null;
        }

        public void Dispose()
        {
            FrontGraphics.Dispose();
            BackGraphics.Dispose();
            FrontBuffer.Dispose();
            BackBuffer.Dispose();
            Processor.Stop();
        }

        private Graphics CreateGraphics(Bitmap bitmap)
        {
            var graphics = Graphics.FromImage(bitmap);
            graphics.PixelOffsetMode = PixelOffsetMode.Half;
            graphics.SmoothingMode = SmoothingMode.HighSpeed;
            graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.CompositingQuality = CompositingQuality.HighSpeed;
            graphics.Clip = new Region(new Rectangle(0, 0, Width, Height));
            return graphics;
        }

        public void SetBackgroundColor(Color color)
        {
            BackgroundColor = color;
            DirtyLock.NotifyDirty();
        }

        public void Invalidate() => DirtyLock.NotifyDirty();
    }
}
