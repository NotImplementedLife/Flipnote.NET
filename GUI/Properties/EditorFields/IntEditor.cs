using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(int))]
    public class IntEditor : NumericUpDown, IPropertyEditorControl
    {
        protected object m_oSelection = null;
        protected IWindowsFormsEditorService m_iwsService = null;
        public IntEditor(object selection, IWindowsFormsEditorService edsvc)
        {
            m_oSelection = selection;
            m_iwsService = edsvc;
        }

        public object Selection => m_oSelection;        


        public IntEditor()
        {
            Minimum = int.MinValue;
            Maximum = int.MaxValue;
            Validating += IntEditor_LostFocus;
            //LostFocus += ObjectPropertyValueChanged;            
        }

        private void IntEditor_LostFocus(object sender, EventArgs e)
        {
            m_oSelection = ObjectPropertyValue;
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }

        public object ObjectPropertyValue { get => (int)Value; set => Value = (int)value; }

        public event EventHandler ObjectPropertyValueChanged;
        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }

    }
}
