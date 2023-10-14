using FlipnoteDotNet.GUI.Canvas.Drawing;
using System;
using System.Drawing;

namespace FlipnoteDotNet.GUI.Canvas.Components
{
    public class CanvasComponent : ICanvasComponent
    {
        private Rectangle _Bounds;

        public virtual Rectangle Bounds
        {
            get => _Bounds;
            set
            {
                _Bounds = value;
                OnBoundsChanged();
            }            
        }

        public virtual Point Location
        {
            get => _Bounds.Location;
            set
            {
                _Bounds.Location = value;
                OnBoundsChanged();
            }
        }
        public virtual Size Size
        {
            get => _Bounds.Size;
            set
            {
                _Bounds.Size = value;
                OnBoundsChanged();
            }            
        }
        public bool IsFixed { get; set; } = false;
        public virtual bool IsResizeable { get; set; } = false;

        public event EventHandler BoundsChanged;

        public virtual void OnPaint(CanvasGraphics g)
        {
            g.DrawRectangle(new Pen(Brushes.Gray, 2) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash }, Bounds);
        }

        protected virtual void OnBoundsChanged()
        {
            BoundsChanged?.Invoke(this, new EventArgs());
        }
    }
}
