using PPMLib.Data;
using PPMLib.IO;
using PPMLib.IO.JPEG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test.Properties;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var mat = new Matrix();
            mat.Translate(50, 50);
            mat.Rotate(45);
            mat.Scale(0.5f, 0.5f);
            mat.Translate(-50, -50);

            e.Graphics.Transform = mat;

            e.Graphics.FillRectangle(Brushes.Red, 0, 0, 100, 100);            
            

        }
    }
}
