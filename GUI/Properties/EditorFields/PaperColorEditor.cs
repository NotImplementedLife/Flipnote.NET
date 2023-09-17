using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Controls;
using PPMLib.Data;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(FlipnotePaperColor))]
    internal class PaperColorEditor : EnumComboBox<FlipnotePaperColor>, IPropertyEditorControl, IDataGridViewEditingControl
    {
        public PaperColorEditor()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            SelectedIndexChanged += PaperColorEditor_SelectedIndexChanged;
            ItemHeight = 20;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += PaperColorEditor_DrawItem;
        }

        private void PaperColorEditor_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush bgBrush;            
            Brush fgBrush;
            var value = Values[e.Index].EnumInstance;

            switch (value)
            {
                case FlipnotePaperColor.Black:
                    bgBrush = Colors.FlipnoteBlack.GetBrush();
                    fgBrush = Colors.FlipnoteWhite.GetBrush();
                    break;
                default:
                    bgBrush = Colors.FlipnoteWhite.GetBrush();
                    fgBrush = Colors.FlipnoteBlack.GetBrush();
                    break;
            }

            e.DrawBackground();

            var rect = e.Bounds.GetPaddedContent(3);

            e.Graphics.FillRectangle(bgBrush, rect);

            e.Graphics.DrawString(Enum.GetName(typeof(FlipnotePaperColor), value), Font, fgBrush, rect);


            e.DrawFocusRectangle();
        }

        public object ObjectPropertyValue { get => SelectedEnumItem; set => SelectedEnumItem = (FlipnotePaperColor)value; }
        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }

        public event EventHandler ObjectPropertyValueChanged;

        private void PaperColorEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }



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
