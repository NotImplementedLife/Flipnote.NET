using FlipnoteDotNet.Attributes;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(string))]
    internal class StringEditor : TextBox, IPropertyEditorControl, IDataGridViewEditingControl
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

        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }


        #region DataGridView
        public DataGridView EditingControlDataGridView { get; set; }
        public object EditingControlFormattedValue
        {
            get => ObjectPropertyValue;
            set
            {
                ObjectPropertyValue = value;
            }
        }
        public int EditingControlRowIndex { get; set; }
        public bool EditingControlValueChanged { get; set; }
        public Cursor EditingPanelCursor => base.Cursor;
        public bool RepositionEditingControlOnValueChange => false;

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle) { }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return true;
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }
        #endregion DataGridView
    }
}
