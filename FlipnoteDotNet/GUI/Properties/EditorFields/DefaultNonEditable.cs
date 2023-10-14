using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    internal class DefaultNonEditable : TextBox, IPropertyEditorControl
    {        
        public DefaultNonEditable()
        {
            ReadOnly = true;
        }

        private object Value;
        public object ObjectPropertyValue 
        { 
            get => Value;
            set { Value = value; Text = Value?.ToString() ?? "null"; }
        }
        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }

#pragma warning disable 0067 // event is never used
        public event EventHandler ObjectPropertyValueChanged;
#pragma warning restore 0067
    }
}
