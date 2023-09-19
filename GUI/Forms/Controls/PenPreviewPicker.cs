using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Paint;
using PPMLib.Data;
using System;
using System.Drawing;
using System.Windows.Forms;
using static FlipnoteDotNet.Constants;

namespace FlipnoteDotNet.GUI.Forms.Controls
{
    public partial class PenPreviewPicker : UserControl, IPaintContextEditor
    {
        public PenPreviewPicker()
        {
            InitializeComponent();
            
            RedColor1Box.BackColor = RedColor2Box.BackColor = Colors.FlipnoteRed;
            BlueColor1Box.BackColor = BlueColor2Box.BackColor = Colors.FlipnoteBlue;

            RedColor1Box.Tag = RedColor2Box.Tag = FlipnotePen.Red;
            BlueColor1Box.Tag = BlueColor2Box.Tag = FlipnotePen.Blue;
            InversePaperColor1Box.Tag = InversePaperColor2Box.Tag = FlipnotePen.PaperInverse;

        }

        public PaintContext PaintContext { get; private set; }

        public void LoadPaintContext(PaintContext pc)
        {
            PaintContext = pc;            

            switch (pc.Pen1)
            {
                case FlipnotePen.Red: RedColor1Box.Checked = true; break;
                case FlipnotePen.Blue: BlueColor1Box.Checked = true; break;
                default: InversePaperColor1Box.Checked = true; break;
            }
            switch (pc.Pen2)
            {
                case FlipnotePen.Red: RedColor2Box.Checked = true; break;
                case FlipnotePen.Blue: BlueColor2Box.Checked = true; break;
                default: InversePaperColor2Box.Checked = true; break;
            }
            
            if (pc.PenValue == 1) Pen1Box.Checked = true;
            else Pen2Box.Checked = true;

            RedColor1Box.CheckedChanged += Color1Box_CheckedChanged;
            BlueColor1Box.CheckedChanged += Color1Box_CheckedChanged;
            InversePaperColor1Box.CheckedChanged += Color1Box_CheckedChanged;

            RedColor2Box.CheckedChanged += Color2Box_CheckedChanged;
            BlueColor2Box.CheckedChanged += Color2Box_CheckedChanged;
            InversePaperColor2Box.CheckedChanged += Color2Box_CheckedChanged;
        }

        private void Color1Box_CheckedChanged(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if(btn.Checked)
                PaintContext.Pen1 = (FlipnotePen)(btn.Tag);
        }
        private void Color2Box_CheckedChanged(object sender, EventArgs e)
        {
            var btn = sender as RadioButton;
            if (btn.Checked)
                PaintContext.Pen2 = (FlipnotePen)(btn.Tag);
        }

        private void Pen1Box_CheckedChanged(object sender, EventArgs e)
        {
            Colors1Group.Enabled = true;
            Colors2Group.Enabled = false;
            PaintContext.PenValue = 1;
        }

        private void Pen2Box_CheckedChanged(object sender, EventArgs e)
        {
            Colors2Group.Enabled = true;
            Colors1Group.Enabled = false;
            PaintContext.PenValue = 2;
        }

        private void InversePaperColorBox_Paint(object sender, PaintEventArgs e)
        {
            var ctr = sender as Control;
            int p = 4;
            var tl = new Point(p, p);
            var tr = new Point(ctr.Width - p - 1, p);
            var bl = new Point(p, ctr.Height - p);
            var br = new Point(ctr.Width - p - 1, ctr.Height - p);

            using (var black = Colors.FlipnoteBlack.GetBrush())
            using (var white = Colors.FlipnoteWhite.GetBrush())
            {                
                e.Graphics.FillPolygon(black, new Point[] { tl, tr, bl });
                e.Graphics.FillPolygon(white, new Point[] { bl, br, tr });
            }                        
        }        
    }
}
