using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.PropertyEditor.Editors
{
    internal class IntegerBox : Editor<int>
    {
        public override int RequestedTextRows => 1;

        private readonly StringFormat StringFormat;

        public IntegerBox() : base(invalidateAfterValueChanged: true) 
        {
            StringFormat = new StringFormat(StringFormat.GenericTypographic);
            StringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            StringFormat.LineAlignment = StringAlignment.Center;
        }

        public override void OnPaint(Graphics g, Font font)
        {
            g.DrawString(fValue.ToString(), font, Brushes.Black, g.ClipBounds, StringFormat);
        }

        public override void OnFocus()
        {
            var tb = new System.Windows.Forms.NumericUpDown
            {
                Value = fValue,
                Tag = new IEditor.SummonedControlStats(this, new Rectangle(0, 0, 0, 0))
            };
            SummonControl(tb);
        }

        public override void OnFocusLost()
        {
            if (SummonedControl != null)
            {
                Value = (int)(SummonedControl as NumericUpDown).Value;
                DisposeControl();
                TriggerUserValueChanged();
            }
        }
    }
}
