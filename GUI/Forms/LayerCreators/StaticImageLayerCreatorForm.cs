using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms.LayerCreators
{
    public partial class StaticImageLayerCreatorForm : Form, ILayerCreatorForm
    {
        public StaticImageLayerCreatorForm()
        {
            InitializeComponent();
        }

        public ILayer Layer { get; set; }

        private void AddLayerButton_Click(object sender, EventArgs e)
        {
            Layer = new StaticImageLayer(VisualSourceEditorControl.GetVisualSourceFromCanvas());
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
