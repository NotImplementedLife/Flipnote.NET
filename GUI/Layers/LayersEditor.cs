using FlipnoteDotNet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Layers
{
    public partial class LayersEditor : UserControl
    {
        public LayersEditor()
        {
            InitializeComponent();            
        }

        private Sequence _Sequence=null;

        public Sequence Sequence
        {
            get => _Sequence;
            set
            {
                _Sequence = value;
                ReloadSequence();
            }
        }


        public void ReloadSequence()
        {
            LayersListBox.SelectedIndexChanged -= LayersListBox_SelectedIndexChanged;
            LayersListBox.DataSource = null;
            LayersListBox.DataSource = Sequence?.Layers;
            LayersListBox.SelectedIndexChanged += LayersListBox_SelectedIndexChanged;
            LayersListBox.SelectedIndex = -1;
        }

        public void ClearSelection()
        {
            LayersListBox.SelectedIndex = -1;
        }

        private void LayersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveLayerButton.Enabled = LayersListBox.SelectedIndex >= 0;

            var selection = LayersListBox.SelectedItem;
            if (selection is ILayer layer)
                SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(layer));
            else
                SelectionChanged?.Invoke(this, new SelectionChangedEventArgs(null));

        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

        public class SelectionChangedEventArgs
        {            
            public ILayer Layer { get; }

            public SelectionChangedEventArgs(ILayer layer)
            {                
                Layer = layer;
            }
        }

        private void LayersListBox_DataSourceChanged(object sender, EventArgs e)
        {
            LayersListBox.ClearSelected();            
        }
    }
}
