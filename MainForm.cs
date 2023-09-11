using FlipnoteDotNet.GUI.Canvas.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Canvas.CanvasComponents.Add(new SimpleRectangle(new Rectangle(0, 0, 256, 192)));
        }
    }
}
