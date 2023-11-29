using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.PropertyEditorFields
{
    [PropertyEditorControl(typeof(float))]
    public class FloatEditor : NumericUpDown, IPropertyEditorControl
    {

        public FloatEditor()
        {
            Minimum = -1000;
            Maximum = 1000;
            DecimalPlaces = 6;
            ValueChanged += FloatEditor_ValueChanged;
        }

        private void FloatEditor_ValueChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }

        public object ObjectPropertyValue { get => (float)Value; set => Value = (decimal)(float)value; }

        public event EventHandler ObjectPropertyValueChanged;       
        public PropertyInfo Property { get; set; }

    }
}
