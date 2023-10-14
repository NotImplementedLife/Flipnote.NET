using FlipnoteDotNet.Attributes;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
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
        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }

    }
}
