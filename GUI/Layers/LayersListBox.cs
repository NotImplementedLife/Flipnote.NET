using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Extensions;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;
using static FlipnoteDotNet.Constants;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System;
using System.ComponentModel.Design;

namespace FlipnoteDotNet.GUI.Layers
{
    [ToolboxItem(true)]
    public class LayersListBox : ListBox
    {
        public LayersListBox()
        {
            LayersBinding = new BindingList<ILayer>();
            DataSource = LayersBinding;
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 50;            
        }
        BindingList<ILayer> LayersBinding;

        public void LoadLayers(IEnumerable<ILayer> layers)
        {
            IsBindingListUpdating = true;
            ClearSelected();
            SelectedIndex = -1;
            LayersBinding.Where(l => l is IDisplayLayer).ForEach(_ => (_ as IDisplayLayer).DisplayChanged -= Layer_DisplayChanged);
            LayersBinding.Clear();
            if (layers != null)
            {
                DataSource = null;
                foreach (var layer in layers)
                {
                    if (layer is IDisplayLayer displayLayer)
                        displayLayer.DisplayChanged += Layer_DisplayChanged;                    
                    LayersBinding.Add(layer);
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
                    Color.White, Colors.FlipnoteThemeMainColor);

            e.DrawBackground();
            if (e.Index >= 0 && e.Index < Items.Count)
            {
                var displayItem = Items[e.Index];                

                if (displayItem is IDisplayLayer displayLayer)
                {
                    var name = displayLayer.DisplayName;
                    if (string.IsNullOrWhiteSpace(name)) name = "Layer";                    

                    var textSize = e.Graphics.MeasureString(name, Font);
                    
                    var y = (int)(e.Bounds.Top + (e.Bounds.Height - textSize.Height) / 2);

                    using (var thumbnail = displayLayer.GetDisplayThumbnail())
                        e.Graphics.DrawImageUnscaled(thumbnail, 30, e.Bounds.Top + 5);
                    e.Graphics.DrawRectangle(Colors.FlipnoteThemeMainColor.GetPen(), 30, e.Bounds.Top + 5, 40, 40);                    

                    e.Graphics.DrawString($"{e.Index + 1}.", Font, e.ForeColor.GetBrush(), 5, y);
                    e.Graphics.DrawString(name, Font, e.ForeColor.GetBrush(), 75, y);
                }
                else if (displayItem is ILayer layer)
                {
                    var textSize = e.Graphics.MeasureString("[=Layer=]", Font);
                    var y = (int)(e.Bounds.Top + (e.Bounds.Height - textSize.Height) / 2);
                    e.Graphics.DrawString("[=Layer=]", Font, e.ForeColor.GetBrush(), 60, y);
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
                }                               
            }
            e.DrawFocusRectangle();
        }

        public void SelectLayer(ILayer layer)
        {
            SelectedIndex = LayersBinding.IndexOf(layer);
        }
    }
}
