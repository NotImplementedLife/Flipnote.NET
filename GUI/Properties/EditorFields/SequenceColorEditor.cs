using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Controls;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    internal class SequenceColorEditor : StaticComboBox<Color>, IPropertyEditorControl
    {
        public SequenceColorEditor() : base(Colors)
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            SelectedIndexChanged += SequenceColorEditor_SelectedIndexChanged; ;
            ItemHeight = 20;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += SequenceColorEditor_DrawItem; ;
        }

        private void SequenceColorEditor_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                e.DrawBackground();
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);
                e.Graphics.DrawString("Error!", Font, Brushes.Red, e.Bounds);
                e.DrawFocusRectangle();
                return;
            }          

            var color = Values[e.Index].Instance;

            e.DrawBackground();
            var rect = e.Bounds.GetPaddedContent(3);
            e.Graphics.FillRectangle(color.GetBrush(), rect);            
            e.DrawFocusRectangle();
        }

        private void SequenceColorEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }

        private static Color[] Colors = new Color[]
        {
            Color.Blue, Color.DodgerBlue, Color.DarkBlue,
            Color.Red, Color.DarkRed,
            Color.Green, Color.DarkGreen,
            Color.OrangeRed, Color.DarkOrange,
            Color.DarkKhaki
        };

        public event EventHandler ObjectPropertyValueChanged;

        public object ObjectPropertyValue { get => SelectedValueItem; set => SelectedValueItem = (Color)value; }
        public KeyFramesEditor KeyframesEditor { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
