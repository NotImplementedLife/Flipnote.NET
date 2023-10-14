using FlipnoteDotNet.GUI.Controls;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    internal class EnumEditorField<T> : EnumComboBox<T>, IPropertyEditorControl where T:struct, IConvertible
    {
        public EnumEditorField()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            SelectedIndexChanged += EnumEditorField_SelectedIndexChanged;            
        }

        private void EnumEditorField_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }

        public object ObjectPropertyValue { get => SelectedEnumItem; set => SelectedEnumItem = (T)value; }
        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }

        public event EventHandler ObjectPropertyValueChanged;
    }
}
