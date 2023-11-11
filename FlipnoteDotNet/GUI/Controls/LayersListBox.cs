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
            ItemHeight = 50;
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private BindingList<LayerRef> LayersBinding;

        public void LoadLayers(IEntityReference<Sequence> sequence)
        {
            IsBindingListUpdating = true;
            ClearSelected();
            SelectedIndex = -1;
            LayersBinding.Clear();
            if (sequence != null) 
            {
                DataSource = null;
                foreach (var layer in sequence.Entity.Layers) 
                {                    
                    LayersBinding.Add(new LayerRef(layer.Id, layer.Entity.Name));
                }
                DataSource = LayersBinding;
                LayersBinding.ResetBindings();
            }
            IsBindingListUpdating = false;            
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

        private void SelectLayer(LayerRef layer)
        {
            SelectedIndex = LayersBinding.IndexOf(layer);
        }
        private LayerRef SelectedLayer => SelectedIndex < 0 ? null : LayersBinding[SelectedIndex];
    }
}
