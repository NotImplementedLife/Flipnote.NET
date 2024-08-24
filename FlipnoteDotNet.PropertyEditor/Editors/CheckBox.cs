

using System.Diagnostics;

namespace FlipnoteDotNet.PropertyEditor.Editors
{
    public class CheckBox : Editor<bool>
    {
        public override int RequestedTextRows => 1;

        private bool MsDown = false;
        public override void OnMouseDown(MouseButtons buttons, int x, int y) 
        {
            if ((buttons & MouseButtons.Left) != 0)
                MsDown = true;
        }        
        public override void OnMouseUp(MouseButtons buttons, int x, int y)
        {
            //Debug.WriteLine($"MsUp {Name}");
            if (MsDown && (buttons & MouseButtons.Left) != 0)
            {
                MsDown = false;
                fValue = !fValue;
                TriggerUserValueChanged();                
                Invalidate();
            }
        }
        public override void OnPaint(Graphics g, Font font)
        {
            int x0 = (int)g.ClipBounds.X, y0 = (int)g.ClipBounds.Y;
            int w = (int)g.ClipBounds.Width, h = (int)g.ClipBounds.Height;
            int l = Math.Max(16, 3 * Math.Min(w, h) / 4 - 2);
            int x = x0 + 2;
            int y = y0 + (h - l) / 2;

            var state = ButtonState.Flat;
            if (fValue == true) state |= ButtonState.Checked;
            ControlPaint.DrawCheckBox(g, x,y,l,l, state);          
        }        
    }
}
