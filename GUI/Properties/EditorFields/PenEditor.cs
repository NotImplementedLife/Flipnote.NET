using FlipnoteDotNet.Attributes;
using PPMLib.Data;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FlipnoteDotNet.GUI.Controls;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using Brushes = System.Drawing.Brushes;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(FlipnotePen))]
    internal class PenEditor : EnumComboBox<FlipnotePen>, IPropertyEditorControl, IDataGridViewEditingControl
    {
        public PenEditor()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            SelectedIndexChanged += PaperColorEditor_SelectedIndexChanged;
            ItemHeight = 20;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += PaperColorEditor_DrawItem;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnDrawItem(new DrawItemEventArgs(e.Graphics, Font, e.ClipRectangle, 0, DrawItemState.Default));
        }

        private void PaperColorEditor_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                e.DrawBackground();
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                e.Graphics.DrawString("Error!", Font, Brushes.Red, e.Bounds);
                e.DrawFocusRectangle();
                return;
            }

            Brush bgBrush;
            Brush fgBrush;            

            var value =  Values[e.Index].EnumInstance;

            switch (value)
            {
                case FlipnotePen.Red:
                    bgBrush = Colors.FlipnoteRed.GetBrush();
                    fgBrush = Brushes.White;
                    break;
                case FlipnotePen.Blue:
                    bgBrush = Colors.FlipnoteBlue.GetBrush();
                    fgBrush = Brushes.White;
                    break;
                default:
                    bgBrush = Color.Black.GetBrush();
                    fgBrush = Brushes.White;
                    break;
            }

            e.DrawBackground();
            var rect = e.Bounds.GetPaddedContent(3);
            e.Graphics.FillRectangle(bgBrush, rect);
            e.Graphics.DrawString(Enum.GetName(typeof(FlipnotePen), value), Font, fgBrush, rect);
            e.DrawFocusRectangle();
        }

        public object ObjectPropertyValue { get => SelectedEnumItem; set => SelectedEnumItem = (FlipnotePen)value; }
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
