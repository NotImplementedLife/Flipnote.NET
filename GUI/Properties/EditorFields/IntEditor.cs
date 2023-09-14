﻿using FlipnoteDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties.EditorFields
{
    [PropertyEditorControl(typeof(int))]
    internal class IntEditor : NumericUpDown, IPropertyEditorControl
    {

        public IntEditor()
        {
            Minimum = int.MinValue;
            Maximum = int.MaxValue;
            Validating += IntEditor_LostFocus;
            //LostFocus += ObjectPropertyValueChanged;            
        }

        private void IntEditor_LostFocus(object sender, EventArgs e)
        {
            ObjectPropertyValueChanged?.Invoke(this, new EventArgs());
        }

        public object ObjectPropertyValue { get => (int)Value; set => Value = (int)value; }

        public event EventHandler ObjectPropertyValueChanged;
        public Panel KeyframesPanel { get; set; }
        public bool IsTimeDependent { get; set; }
        public PropertyInfo Property { get; set; }
    }
}