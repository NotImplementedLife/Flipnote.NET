using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    public partial class FormBasedEditor : UserControl, IPropertyEditorControl
    {
        public FormBasedEditor(Type formType)
        {
            InitializeComponent();
            FormType = formType;
        }

        Type FormType;
        IObjectHolderDialog DialogForm;

        private object _ObjectPropertyValue;
        public object ObjectPropertyValue 
        {
            get => _ObjectPropertyValue;
            set
            {
                _ObjectPropertyValue = value;
                DisplayBox.Text = _ObjectPropertyValue?.ToString() ?? "(empty)";
            }
        }        
        public PropertyInfo Property { get; set; }       

        public event EventHandler ObjectPropertyValueChanged;

        private void OpenFormButton_Click(object sender, EventArgs e)
        {
            DialogForm = Activator.CreateInstance(FormType) as IObjectHolderDialog;

            DialogForm.ObjectValue = ObjectPropertyValue;
            if(DialogForm.ShowDialog()==DialogResult.OK)
            {
                ObjectPropertyValue = DialogForm.ObjectValue;
                ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
