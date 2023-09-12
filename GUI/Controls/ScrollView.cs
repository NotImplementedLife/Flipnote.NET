using FlipnoteDotNet.Extensions;
using System;
using System.ComponentModel;
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
    }
}
