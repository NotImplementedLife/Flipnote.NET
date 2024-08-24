using System.Diagnostics;

namespace FlipnoteDotNet.Core.GUI
{
    public class VirtualScroller
    {
        private readonly ScrollState HScrollState = new ScrollState();
        private readonly ScrollState VScrollState = new ScrollState();
        private Control Control;

        public VirtualScroller(bool horizontal=false, bool vertical=false)
        {
            HScrollState.Enabled = horizontal;
            VScrollState.Enabled = vertical;
        }

        public void Attach(Control control)
        {
            if (Control == control) return;
            Detach();
            Control = control;
            Control.Paint += Control_Paint;
            Control.Resize += Control_Resize;
            Control.MouseDown += Control_MouseDown;
            Control.MouseMove += Control_MouseMove;
            Control.MouseUp += Control_MouseUp;
            Control.MouseLeave += Control_MouseLeave;

            UpdateState();
        }

        public void Detach()
        {
            if (Control == null) return;
            Control.MouseDown -= Control_MouseDown;
            Control.Paint -= Control_Paint;
            Control.Resize -= Control_Resize;
            Control.MouseDown -= Control_MouseDown;
            Control.MouseMove -= Control_MouseMove;
            Control.MouseUp -= Control_MouseUp;
            Control.MouseLeave -= Control_MouseLeave;
            Control = null;
        }

        private void UpdateState()
        {
            HScrollState.UpdateH(Control);
            VScrollState.UpdateV(Control);            
        }

        public event EventHandler<int> HScrollChanged
        {
            add => HScrollState.ScrollChanged += value;
            remove => HScrollState.ScrollChanged -= value;
        }

        public event EventHandler<int> VScrollChanged
        {
            add => VScrollState.ScrollChanged += value;
            remove => VScrollState.ScrollChanged -= value;
        }

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            if (HScrollState.Enabled && HScrollState.ThumbRectangle.Contains(e.Location))
            {
                HScrollState.MsDown = true;
                HScrollState.DownPos = e.Location.X;
                HScrollState.ThumbPos = HScrollState.ThumbRectangle.X;
            }
            else if (VScrollState.Enabled && VScrollState.ThumbRectangle.Contains(e.Location)) 
            {                
                VScrollState.MsDown = true;
                VScrollState.DownPos = e.Location.Y;
                VScrollState.ThumbPos = VScrollState.ThumbRectangle.Y;
            }            
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (HScrollState.MsDown)
            {
                if (HScrollState.Hidden) return;
                int dx = e.Location.X - HScrollState.DownPos;
                int scrollX = (int)((HScrollState.ThumbPos + dx) * HScrollState.Scale);
                int w = HScrollState.ContentLength - Control.Width;
                scrollX = scrollX < 0 ? 0 : scrollX > w ? w : scrollX;
                HScrollState.Position = scrollX;
                HScrollState.UpdateH(Control);
                HScrollState.TriggerScrollChanged();
                Control.Invalidate();
            }
            else if (VScrollState.MsDown)
            {
                if (VScrollState.Hidden) return;
                int dy = e.Location.Y - VScrollState.DownPos;
                int scrollY = (int)((VScrollState.ThumbPos + dy) * VScrollState.Scale);                
                int h = VScrollState.ContentLength - Control.Height;
                scrollY = scrollY < 0 ? 0 : scrollY > h ? h : scrollY;
                VScrollState.Position = scrollY;                
                VScrollState.UpdateV(Control);
                VScrollState.TriggerScrollChanged();
                Control.Invalidate();
            }
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            HScrollState.MsDown = VScrollState.MsDown = false;            
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            HScrollState.MsDown = VScrollState.MsDown = false;
        }

        public void SetScrollableWidth(int width)
        {
            HScrollState.ContentLength = width;
            HScrollState.UpdateH(Control);
            Control.Invalidate();
        }

        public void SetScrollableHeight(int height) 
        {
            VScrollState.ContentLength = height;
            VScrollState.UpdateV(Control);
            Control.Invalidate();
        }

        private void Control_Paint(object sender, PaintEventArgs e)
        {
            HScrollState.Draw(e.Graphics, Brushes.Transparent, Brushes.DarkOrange);
            VScrollState.Draw(e.Graphics, Brushes.Transparent, Brushes.DarkOrange);            
        }

        private void Control_Resize(object sender, EventArgs e)
        {
            UpdateState();
            Control.Invalidate();
        }

    }
}
