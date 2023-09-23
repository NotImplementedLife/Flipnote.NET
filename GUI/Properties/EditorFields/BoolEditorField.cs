using FlipnoteDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(bool))]
    internal class BoolEditorField : CheckBox, IPropertyEditorControl
    {
        public BoolEditorField()
        {
            CheckedChanged += BoolEditorField_CheckedChanged;
        }

        private void BoolEditorField_CheckedChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }

        public object ObjectPropertyValue { get => Checked; set => Checked = (bool)value; }
        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }

        public event EventHandler ObjectPropertyValueChanged;
    }
}
