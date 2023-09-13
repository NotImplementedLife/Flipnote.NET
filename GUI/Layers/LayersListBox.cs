using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Extensions;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;

namespace FlipnoteDotNet.GUI.Layers
{
    [ToolboxItem(true)]
    public class LayersListBox : ListBox
    {
        public LayersListBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 50;            
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
                if (displayItem is Sequence.Element element)
                    displayItem = element.Layer;


                if (displayItem is IDisplayLayer displayLayer)
                {
                    var name = displayLayer.DisplayName;
                    if (string.IsNullOrWhiteSpace(name)) name = "Layer";                    

                    var textSize = e.Graphics.MeasureString(name, Font);
                    
                    var y = (int)(e.Bounds.Top + (e.Bounds.Height - textSize.Height) / 2);

                    e.Graphics.DrawString($"{e.Index + 1}.", Font, e.ForeColor.GetBrush(), 5, y);
                    e.Graphics.DrawString(name, Font, e.ForeColor.GetBrush(), 60, y); ;
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
    }
}
