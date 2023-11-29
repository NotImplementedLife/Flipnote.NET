﻿using PPMLib.Data;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FlipnoteDotNet.Commons.GUI.Controls.Primitives;
using PPMLib.Winforms;
using FlipnoteDotNet.Commons.GUI;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(FlipnotePaperColor))]
    internal class PaperColorEditor : EnumComboBox<FlipnotePaperColor>, IPropertyEditorControl
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
                    bgBrush = FlipnoteBrushes.Black;
                    fgBrush = FlipnoteBrushes.White;
                    break;
                default:
                    bgBrush = FlipnoteBrushes.White;
                    fgBrush = FlipnoteBrushes.Black;
                    break;
            }

            e.DrawBackground();

            var rect = e.Bounds.GetPaddedContent(3);

            e.Graphics.FillRectangle(bgBrush, rect);

            e.Graphics.DrawString(Enum.GetName(typeof(FlipnotePaperColor), value), Font, fgBrush, rect);


            e.DrawFocusRectangle();
        }

        public object ObjectPropertyValue { get => SelectedEnumItem; set => SelectedEnumItem = (FlipnotePaperColor)value; }        
        public PropertyInfo Property { get; set; }

        public event EventHandler ObjectPropertyValueChanged;

        private void PaperColorEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }
    }
}