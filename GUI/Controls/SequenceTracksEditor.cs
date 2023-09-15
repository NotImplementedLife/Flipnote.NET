using FlipnoteDotNet.Data;
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

namespace FlipnoteDotNet.GUI.Controls
{
    public partial class SequenceTracksEditor : UserControl
    {
        public SequenceTracksEditor()
        {
            InitializeComponent();
            ToolStrip.Renderer = new ToolStripEmptyRenderer();
        }

        internal SequenceTracksViewer Viewer => SequenceTracksViewer;

        private void AddNewSequenceButton_Click(object sender, EventArgs e)
        {
            if(AddNewSequenceButton.Checked)
            {
                Viewer.StartSequenceCreateMode();
            }
            else
            {
                Viewer.EndSequenceCreateMode();
            }
        }

        private void SequenceTracksViewer_SequenceCreateModeEnded(object sender, EventArgs e)
        {
            AddNewSequenceButton.Checked = false;
        }

        private void SequenceTracksViewer_SelectedElementChanged(object sender, EventArgs e)
        {
            SelectedElementChanged?.Invoke(this, SequenceTracksViewer.SelectedElement);
        }

        public event EventHandler<Sequence> SelectedElementChanged;
        public event EventHandler CurrentFrameChanged;

        private void SequenceTracksViewer_CurrentFrameChanged(object sender, EventArgs e)
        {
            CurrentFrameChanged?.Invoke(this, new EventArgs());
        }
    }
}
