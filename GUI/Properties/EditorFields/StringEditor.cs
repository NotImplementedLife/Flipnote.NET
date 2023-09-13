using FlipnoteDotNet.Attributes;
using System;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(string))]
    internal class StringEditor : TextBox, IPropertyEditorControl
    {
        public StringEditor()
        {
            LostFocus += StringEditor_LostFocus;
        }

        private void StringEditor_LostFocus(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }

        public object ObjectPropertyValue
        {
            get => Text;
            set => Text = value as string;
        }

        public event EventHandler ObjectPropertyValueChanged;

        public Panel KeyframesPanel { get; set; }
    }
}
