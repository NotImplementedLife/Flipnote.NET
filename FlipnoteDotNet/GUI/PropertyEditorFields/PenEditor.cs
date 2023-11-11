﻿using FlipnoteDotNet.Commons.GUI;
using FlipnoteDotNet.Commons.GUI.Controls.Primitives;
using PPMLib.Data;
using PPMLib.Winforms;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Brushes = System.Drawing.Brushes;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(FlipnotePen))]
    internal class PenEditor : EnumComboBox<FlipnotePen>, IPropertyEditorControl
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
                    bgBrush = FlipnoteBrushes.Red;
                    fgBrush = Brushes.White;
                    break;
                case FlipnotePen.Blue:
                    bgBrush = FlipnoteBrushes.Blue;
                    fgBrush = Brushes.White;
                    break;
                default:
                    bgBrush = Brushes.Black;
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
        public PropertyInfo Property { get; set; }

        public event EventHandler ObjectPropertyValueChanged;

        private void PaperColorEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }
    }
}
