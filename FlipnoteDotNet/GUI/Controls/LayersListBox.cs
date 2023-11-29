using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System;
using PPMLib.Winforms;
using FlipnoteDotNet.Data.Entities;
using FlipnoteDotNet.Model.Entities;

namespace FlipnoteDotNet.GUI.Controls
{
    [ToolboxItem(true)]
    public class LayersListBox : ListBox
    {
        class LayerRef
        {
            public int EID;
            public string Name;

            public LayerRef(int eID, string name)
            {
                EID = eID;
                Name = name;
            }
        }

        public LayersListBox()
        {
            LayersBinding = new BindingList<LayerRef>();
            DataSource = LayersBinding;
            DrawMode = DrawMode.OwnerDrawFixed;
            SelectionMode = SelectionMode.One;
            ItemHeight = 50;
            SelectedIndex = -1;            
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private readonly BindingList<LayerRef> LayersBinding;

        public void LoadLayers(IEntityReference<Sequence> sequence)
        {            
            IsBindingListUpdating = true;

            int selectedId = SelectedIndex >= 0 ? LayersBinding[SelectedIndex].EID : -1;
            int selIndex = -1;

            ClearSelected();
            SelectedIndex = -1;
            LayersBinding.Clear();
            if (sequence != null)
            {
                DataSource = null;
                foreach (var layer in sequence.Entity.Layers)
                {
                    if (selectedId == layer.Id) selIndex = LayersBinding.Count;
                    LayersBinding.Add(new LayerRef(layer.Id, layer.Entity.Name));                    
                }
                DataSource = LayersBinding;
                LayersBinding.ResetBindings();
            }
            //SelectionMode = SelectionMode.One;
            IsBindingListUpdating = false;
            SelectedIndex = selIndex;
        }


        private void Layer_DisplayChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Layer display changed!!!");
            Invalidate();
        }

        private bool IsBindingListUpdating = false;

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (IsBindingListUpdating) return;
            base.OnSelectedIndexChanged(e);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected,
                    Color.White, FlipnoteColors.ThemePrimary);

            e.DrawBackground();
            if (e.Index >= 0 && e.Index < Items.Count)
            {
                var displayLayer = Items[e.Index] as LayerRef;

                var name = displayLayer.Name;
                if (string.IsNullOrWhiteSpace(name)) name = "Layer";                    

                var textSize = e.Graphics.MeasureString(name, Font);
                    
                var y = (int)(e.Bounds.Top + (e.Bounds.Height - textSize.Height) / 2);

                using (var foreBrush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString($"{e.Index + 1}.", Font, foreBrush, 5, y);
                    e.Graphics.DrawString(name, Font, foreBrush, 75, y);
                }

                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                //using (var thumbnail = displayLayer.GetDisplayThumbnail())
                //e.Graphics.DrawImageUnscaled(thumbnail, 30, e.Bounds.Top + 5);
                e.Graphics.DrawRectangle(FlipnotePens.ThemePrimary, 30, e.Bounds.Top + 5, 40, 40);
            }
            e.DrawFocusRectangle();
        }

        protected override void OnClick(EventArgs e)
        {
            int index = IndexFromPoint(PointToClient(Cursor.Position));
            if (index != ListBox.NoMatches) 
            {
                UserLayerClick?.Invoke(this, LayersBinding[index].EID);
            }
            base.OnClick(e);
        }

        public void SelectLayer(int layerId)
        {
            for(int i=0;i<LayersBinding.Count;i++)
            {
                if (LayersBinding[i].EID==layerId)
                {
                    SelectedIndex = i;
                    return;
                }
            }
            SelectedIndex = ListBox.NoMatches;
        }
        private LayerRef SelectedLayer => SelectedIndex < 0 ? null : LayersBinding[SelectedIndex];

        public delegate void OnUserLayerClick(object sender, int layerEId);
        public event OnUserLayerClick UserLayerClick;
    }
}
