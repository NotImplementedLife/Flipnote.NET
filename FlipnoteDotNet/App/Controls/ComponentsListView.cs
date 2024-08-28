using FlipnoteDotNet.Canvas.Components;
using System.Diagnostics;

namespace FlipnoteDotNet.App.Controls
{
    public class ComponentsListView : ListBox
    {        
        private CanvasComponent SelectedComponent = null;

        public ComponentsListView()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            ItemHeight = 50;
            SelectedIndex = -1;
            SelectionMode = SelectionMode.None;
        }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);            
        }        

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);
            Brush roomsBrush;

            var state = e.State & ~DrawItemState.Selected;            

            if (Items[e.Index] == SelectedComponent) 
            {
                e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, state, e.ForeColor, Color.Orange);
                roomsBrush = Brushes.White;
            }
            else
            {
                e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index, state, e.ForeColor, BackColor);
                roomsBrush = Brushes.Black;
            }
            
            var linePen = new Pen(SystemBrushes.Control);
            var lineStartPoint = new Point(e.Bounds.Left, e.Bounds.Height + e.Bounds.Top);
            var lineEndPoint = new Point(e.Bounds.Width, e.Bounds.Height + e.Bounds.Top);

            e.Graphics.DrawLine(linePen, lineStartPoint, lineEndPoint);

            // Command the event to draw the appropriate background of the item.
            e.DrawBackground();

            // Here you get the data item associated with the current item being drawed.
            var dataItem = Items[e.Index] as CanvasComponent;
            
            var timeFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);            
            e.Graphics.DrawString(dataItem.GetType().Name, timeFont, Brushes.Black, e.Bounds.Left + 3, e.Bounds.Top + 5);
            
            var roomsFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular);

            
            if ((dataItem?.Name?.Length ?? 0) == 0)
            {
                e.Graphics.DrawString("(unnamed)", roomsFont, roomsBrush, e.Bounds.Left + 3, e.Bounds.Top + 18);
            }
            else
            {
                e.Graphics.DrawString(dataItem.Name, roomsFont, roomsBrush, e.Bounds.Left + 3, e.Bounds.Top + 18);
            }
        }


        public void SetData(IList<CanvasComponent> data)
        {
            SelectedComponent = null;
            DataSource = data;            
        }        
    }
}
