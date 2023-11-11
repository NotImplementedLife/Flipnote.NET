using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.PropertyEditorFields
{
    [PropertyEditorControl(typeof(int))]
    public class IntEditor : NumericUpDown, IPropertyEditorControl
    {             

        public IntEditor()
        {
            Minimum = int.MinValue;
            Maximum = int.MaxValue;
            //Validating += IntEditor_LostFocus;
            //LostFocus += ObjectPropertyValueChanged;            
            ValueChanged += IntEditor_ValueChanged;
        }

        private void IntEditor_ValueChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }        

        public object ObjectPropertyValue { get => (int)Value; set => Value = (int)value; }

        public event EventHandler ObjectPropertyValueChanged;                
        public PropertyInfo Property { get; set; }

    }
}
