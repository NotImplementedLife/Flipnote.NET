﻿using FlipnoteDotNet.Attributes;
using PPMLib.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Windows.Forms;
using FlipnoteDotNet.GUI.Controls;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
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

        private void PaperColorEditor_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush bgBrush;
            Brush fgBrush;
            var value = Values[e.Index].EnumInstance;

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

        public object ObjectPropertyValue { get => SelectedItem; set => SelectedItem = (FlipnotePen)value; }
        public Panel KeyframesPanel { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }

        public event EventHandler ObjectPropertyValueChanged;

        private void PaperColorEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }
    }
}