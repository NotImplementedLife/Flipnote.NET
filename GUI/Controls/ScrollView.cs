using FlipnoteDotNet.Extensions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Controls
{
    [ToolboxItem(true)]
    public partial class ScrollView : UserControl
    {
        public ScrollView()
        {
            InitializeComponent();
            ScrollContainer.EnableDoubleBuffer();                            
        }


        private Size _SurfaceSize = new Size(100, 100);

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size SurfaceSize
        {
            get => _SurfaceSize;
            set
            {
                _SurfaceSize = value;
                RefreshScrollbars();
                ScrollContainer.Invalidate();
            }
        }

        public int ScrollX => HScrollBar.Value;
        public int ScrollY => VScrollBar.Value;

        private void RefreshScrollbars()
        {
            AdjustLayout();            
            UpdateScrollBar(HScrollBar, SurfaceSize.Width, ScrollContainer.Width);
            UpdateScrollBar(VScrollBar, SurfaceSize.Height, ScrollContainer.Height);            
        }


        private void AdjustLayout()
        {
            int w = Width;
            int h = Height;

            bool hsNeeded = SurfaceSize.Width >= w;
            bool vsNeeded = SurfaceSize.Height >= h;

            if (hsNeeded) h -= HScrollBar.Height;
            if (vsNeeded) w -= VScrollBar.Width;

            hsNeeded = SurfaceSize.Width >= w;
            vsNeeded = SurfaceSize.Height >= h;            

            HScrollBar.Visible = CornerFiller.Visible = hsNeeded;
            VScrollbarContainer.Visible = vsNeeded;

            HScrollBar.BringToFront();
            ScrollContainer.BringToFront();            
        }

        private static void UpdateScrollBar(ScrollBar scrollBar, int effectiveMaxValue, int canvasLimit)
        {                            
            scrollBar.Minimum = 0;
            scrollBar.SmallChange = effectiveMaxValue / 20;
            scrollBar.LargeChange = effectiveMaxValue / 10;
            scrollBar.Maximum = Math.Max(0, effectiveMaxValue - canvasLimit) + scrollBar.LargeChange;

            if (!scrollBar.Visible)
                scrollBar.Value = 0;
            //scrollBar.Visible = scrollBar.Maximum != 0;
        }

        private void Container_Resize(object sender, EventArgs e)
        {            
            ScrollContainer.Invalidate();
        }
        
        private void Container_Paint(object sender, PaintEventArgs e)
        {                        
            e.Graphics.FillRectangle(Constants.Brushes.TransparentBackgroundBrush, -ScrollX, -ScrollY, SurfaceSize.Width, SurfaceSize.Height);
            e.Graphics.DrawRectangle(Pens.Red, -ScrollX, -ScrollY, SurfaceSize.Width, SurfaceSize.Height);

            SurfacePaint?.Invoke(sender, e);
        }

        public event PaintEventHandler SurfacePaint;

        public Size ContainerSize => ScrollContainer.Size;
        
        private void ScrollBar_ValueChanged(object sender, EventArgs e) => ScrollContainer.Invalidate();

        private void ScrollView_Resize(object sender, EventArgs e)
        {
            RefreshScrollbars();            
        }

        public void InvalidateSurface() => ScrollContainer.Invalidate();

        // https://www.philosophicalgeek.com/2007/07/27/mouse-tilt-wheel-horizontal-scrolling-in-c/
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.HWnd != this.Handle)
            {
                return;
            }
            switch (m.Msg)
            {
                case Win32Messages.WM_MOUSEHWHEEL:
                    FireMouseHWheel(m.WParam, m.LParam);
                    m.Result = (IntPtr)1;
                    break;
                default:
                    break;
            }
        }
    
        abstract class Win32Messages
        {
            public const int WM_MOUSEHWHEEL = 0x020E;
        }

        public event EventHandler<MouseEventArgs> MouseHWheel;
        protected void FireMouseHWheel(IntPtr wParam, IntPtr lParam)
        {

            int tilt = HiWord(wParam);
            int keys = LoWord(wParam);
            int x = LoWord(lParam);
            int y = HiWord(lParam);
            FireMouseHWheel(MouseButtons.None, 0, x, y, tilt);
        }

        protected void FireMouseHWheel(MouseButtons buttons, int clicks, int x, int y, int delta)
        {
            MouseEventArgs args = new MouseEventArgs(buttons, clicks, x, y, delta);
            MouseHWheel?.Invoke(this, args);
            //Debug.WriteLine($"HSCROLL {x} {y} {delta}");
            HScrollBar.Value = (HScrollBar.Value + delta / SystemInformation.MouseWheelScrollDelta).Clamp(HScrollBar.Minimum, HScrollBar.Maximum);
        }

        private int HiWord(IntPtr x) => (short)((int)(long)x >> 16);
        private int LoWord(IntPtr x) => (short)((int)(long)x & 0xFFFF);
    }
}
