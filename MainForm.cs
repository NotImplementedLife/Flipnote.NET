using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI;
using FlipnoteDotNet.GUI.Canvas.Components;
using FlipnoteDotNet.GUI.Tracks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;

namespace FlipnoteDotNet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();                       

            LeftContainer.Panel1.EnableDoubleBuffer();
            LeftContainer.Panel2.EnableDoubleBuffer();
            RightContainer.Panel1.EnableDoubleBuffer();
            RightContainer.Panel2.EnableDoubleBuffer();

            RightContainer.Panel1.Paint += BottomLine_Paint;
            RightContainer.Panel2.Paint += TopLine_Paint;

            RightContainer.DisableSelectable();

            ToolStrip.Renderer = new ToolStripEmptyRenderer();

            SequenceTracksEditor.ToolStrip.Paint += BackgroundControlPaint;
        }


        SequenceTracksViewer SequenceTrackViewer => SequenceTracksEditor.Viewer;

        private void BottomLine_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            e.Graphics.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(), 0, control.Height - 2, control.Width, control.Height - 2);
        }

        private void TopLine_Paint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            e.Graphics.DrawLine(Colors.FlipnoteThemeMainColor.GetPen(), 0, 2, control.Width, 2);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var frame = new SimpleRectangle(new Rectangle(0, 0, 256, 192));
            frame.Pen = Colors.FlipnoteThemeMainColor.GetPen(2, System.Drawing.Drawing2D.DashStyle.Dash);
            frame.IsFixed = true;

            Canvas.CanvasComponents.Add(new SimpleRectangle(new Rectangle(0, 0, 256, 192)) { Brush = Brushes.White, IsFixed = true });

            Canvas.CanvasComponents.Add(new BitmapComponent(new Bitmap(@"C:\Users\NotImpLife\Desktop\test.jpg"), 50, 70, 100, 100));

            Canvas.CanvasComponents.Add(new SimpleRectangle(new Rectangle(50, 64, 32, 32)));
            Canvas.CanvasComponents.Add(new SimpleRectangle(new Rectangle(100, 26, 64, 32)));
            Canvas.CanvasComponents.Add(frame);
            Canvas.CanvasViewLocation = Point.Empty;

            SequenceTrackViewer.SequenceManager = new Data.SequenceManager(5);            

            SequenceTrackViewer.SequenceManager.GetTrack(0).AddSequence(new Data.Sequence() { Name = "Tralalaala" }, 10, 25);
            SequenceTrackViewer.SequenceManager.GetTrack(1).AddSequence(new Data.Sequence() { Name = "This Sequence", Color = Color.DarkBlue }, 16, 30);


            SequenceTrackViewer.AdjustSurfaceSize();
            SequenceTrackViewer.Invalidate();
        }

        private void BackgroundControlPaint(object sender, PaintEventArgs e)
        {
            var control = sender as Control;
            var pos = GetLocationRelativeToForm(control);

            var dx = pos.X % 16;
            var dy = pos.Y % 16;

            e.Graphics.FillRectangle(Constants.Brushes.GetWindowBackgroundBrush(dx, dy), new Rectangle(Point.Empty, control.Size));
        }

        private Point GetLocationRelativeToForm(Control c)
        {
            if (c == this) return Point.Empty;
            if (c.Parent == this) return c.Location;
            var result = c.Location;
            result.Offset(GetLocationRelativeToForm(c.Parent));
            return result;
        }
    }
}
