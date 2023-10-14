using PPMLib.Data;
using PPMLib.IO;
using System.IO;
using System.Windows.Forms;
using Test.Properties;

namespace Test
{
    public partial class TestForm : Form
    {
        GDIJpegLayerExporter LayerExporter = new GDIJpegLayerExporter(Resources.aeskey01);

        public TestForm()
        {
            InitializeComponent();            
        }

        FlipnoteFrameLayer layer = new FlipnoteFrameLayer();

        bool msDwn = false;
        bool Drw = true;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            msDwn = true;
            Drw = e.Button == MouseButtons.Left;

            var x = e.Location.X / 2;
            var y = e.Location.Y / 2;
            if (x < 0 || x >= 256 || y < 0 || y >= 192) return;

            layer[x, y] = Drw ? 1 : 0;
            panel1.Invalidate();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!msDwn) return;

            var x = e.Location.X / 2;
            var y = e.Location.Y / 2;

            if (x < 0 || x >= 256 || y < 0 || y >= 192) return;

            layer[x, y] = Drw ? 1 : 0;
            panel1.Invalidate();

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            msDwn = false;
        }

        private void panel1_MouseEnter(object sender, System.EventArgs e)
        {
            msDwn = false;
        }

        private void panel1_MouseLeave(object sender, System.EventArgs e)
        {
            msDwn = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            using (var bmp = LayerExporter.LayerToBitmap(layer))
            {
                e.Graphics.DrawImage(bmp, 0, 0, 256 * 2, 192 * 2);
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var exp = LayerExporter.Export(layer);
            File.WriteAllBytes("exp.jpg", exp);
        }
    }
}
