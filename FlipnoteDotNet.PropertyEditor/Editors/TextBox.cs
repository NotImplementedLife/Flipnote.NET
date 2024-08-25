namespace FlipnoteDotNet.PropertyEditor.Editors
{
    public class TextBox : Editor<string>
    {
        public override int RequestedTextRows => 1;
              
        private readonly StringFormat StringFormat;

        public TextBox() : base(invalidateAfterValueChanged:true)
        {
            StringFormat = new StringFormat(StringFormat.GenericTypographic);
            StringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            StringFormat.LineAlignment = StringAlignment.Center;
        }


        public override void OnPaint(Graphics g, Font font)
        {
            g.DrawString(fValue, font, Brushes.Black, g.ClipBounds, StringFormat);
        }        

        public override void OnFocus()
        {
            var tb = new System.Windows.Forms.TextBox
            {
                Text = fValue,
                Tag = new IEditor.SummonedControlStats(this, new Rectangle(0, 0, 0, 0))
            };
            SummonControl(tb);
        }

        public override void OnFocusLost()
        {
            if (SummonedControl != null)
            {
                var oldValue = fValue;
                Value = SummonedControl.Text;
                DisposeControl();
                TriggerUserValueChanged(oldValue, fValue, preview: false);
            }            
        }        
    }
}
