﻿using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.GUI.Forms;
using FlipnoteDotNet.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Layers
{
    public partial class LayersEditor : UserControl
    {
        public LayersEditor()
        {
            InitializeComponent();
            InitAddLayerContextMenu();
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
            AddLayerButton.Enabled = Sequence != null;
            LayersListBox.SelectedIndexChanged -= LayersListBox_SelectedIndexChanged;
            LayersListBox.LoadLayers(Sequence?.Layers);            
            LayersListBox.SelectedIndexChanged += LayersListBox_SelectedIndexChanged;
            LayersListBox.SelectedIndex = -1;

            RemoveLayerButton.Enabled
                = LayerMoveDownButton.Enabled
                = LayerMoveUpButton.Enabled
                = LayersListBox.SelectedIndex >= 0;
        }

        public void ClearSelection()
        {
            LayersListBox.SelectedIndex = -1;
        }

        private void LayersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveLayerButton.Enabled
                = LayerMoveDownButton.Enabled
                = LayerMoveUpButton.Enabled
                = LayersListBox.SelectedIndex >= 0;

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

        private void InitAddLayerContextMenu()
        {
            if (Constants.IsDesignerMode)
                return;
            Constants.Reflection.LayerTypes.ForEach(ltype =>
            {
                var attr = ltype.GetCustomAttribute<LayerAttribute>();
                if (attr == null) return;
                if (attr.CreatorForm == null) return;
                var item = new ToolStripMenuItem();
                item.Text = attr.DisplayName;
                item.Tag = attr;
                item.Click += AddLayerItem_Click;
                AddLayerContextMenuStrip.Items.Add(item);
            });
           
        }

        private void AddLayerItem_Click(object sender, EventArgs e)
        {
            var a = (sender as ToolStripMenuItem).Tag as LayerAttribute;
            var form = Activator.CreateInstance(a.CreatorForm) as ILayerCreatorForm;
            if (form.ShowDialog() == DialogResult.OK)
            {
                var layer = form.Layer;
                Sequence.AddLayer(layer);
                ReloadSequence();
                LayersListChanged?.Invoke(this, new EventArgs());
            }
        }

        private void AddLayerButton_Click(object sender, EventArgs e)
        {
            AddLayerContextMenuStrip.Show(Cursor.Position);
        }        

        private void RemoveLayerButton_Click(object sender, EventArgs e)
        {
            var selLayer = LayersListBox.SelectedLayer;
            if (selLayer == null) return;
            Sequence.Layers.Remove(selLayer);
            ReloadSequence();
            LayersListChanged?.Invoke(this, new EventArgs());
        }

        public event EventHandler LayersListChanged;

        private void LayerMoveUpButton_Click(object sender, EventArgs e)
        {
            if (Sequence == null) return;
            var i = LayersListBox.SelectedIndex;
            if (i < 1) return;
            (Sequence.Layers[i], Sequence.Layers[i - 1]) = (Sequence.Layers[i - 1], Sequence.Layers[i]);
            ReloadSequence();
            LayersListBox.SelectedIndex = i - 1;
            LayersListChanged?.Invoke(this, new EventArgs());
        }

        private void LayerMoveDownButton_Click(object sender, EventArgs e)
        {
            if (Sequence == null) return;
            var i = LayersListBox.SelectedIndex;
            if (i < 0 || i >= Sequence.Layers.Count-1) return;
            (Sequence.Layers[i], Sequence.Layers[i + 1]) = (Sequence.Layers[i + 1], Sequence.Layers[i]);
            ReloadSequence();
            LayersListBox.SelectedIndex = i + 1;
            LayersListChanged?.Invoke(this, new EventArgs());
        }
    }
}
