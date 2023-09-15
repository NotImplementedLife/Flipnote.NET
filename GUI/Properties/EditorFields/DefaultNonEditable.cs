using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        public event EventHandler ObjectPropertyValueChanged;
    }
}
