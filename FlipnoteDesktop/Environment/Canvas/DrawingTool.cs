using FlipnoteDesktop.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlipnoteDesktop.Environment.Canvas
{
    public class DrawingTool
    {
        public FrameCanvasEditor Target { get; private set; } = null;

        public delegate void OnAttached(object o);
        public event OnAttached Attached;

        public delegate void OnDetached(object o);
        public event OnDetached Detached;

        public void Attach(FrameCanvasEditor target)
        {
            Detach();
            Target = target;
            Target.DrawingSurface.PreviewMouseDown += OnMouseDown;
            Target.DrawingSurface.PreviewMouseMove += OnMouseMove;
            Target.DrawingSurface.PreviewMouseUp += OnMouseUp;
            Target.DrawingSurface.MouseLeave += OnMouseLeave;
            Attached?.Invoke(this);
        }        

        public void Detach()
        {
            if(Target!=null)
            {
                Target.DrawingSurface.PreviewMouseDown -= OnMouseDown;
                Target.DrawingSurface.PreviewMouseMove -= OnMouseMove;
                Target.DrawingSurface.PreviewMouseUp -= OnMouseUp;
                Target.ToolOptions.Children.Clear();
                Detached.Invoke(this);
                Target = null;                
            }
        }

        protected virtual void OnMouseLeave(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
        protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e) { }
        protected virtual void OnMouseMove(object sender, MouseEventArgs e) { }
        protected virtual void OnMouseUp(object sender, MouseEventArgs e) { }
    }
}
