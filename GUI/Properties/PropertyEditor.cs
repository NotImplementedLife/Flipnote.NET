using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace FlipnoteDotNet.GUI.Properties
{
    public partial class PropertyEditor : UserControl
    {
        public PropertyEditor()
        {
            InitializeComponent();
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Panel KeyFramesPanel { get; set; }

        private object _Target = null;
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public object Target
        {
            get => _Target;
            set
            {
                _Target = value;
                ReloadFields();
                TargetChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler TargetChanged;

        private List<IPropertyEditorControl> Editors { get; } = new List<IPropertyEditorControl>();

        private void ReloadFields()
        {

            Editors.ForEach(e => e.ObjectPropertyValueChanged -= PropEditorControl_ObjectPropertyValueChanged);
            Editors.Clear();
            Controls.Clear();
            if (Target == null) return;
            var properties = Target.GetType().GetAllPublicProperties()
                .Where(p => Attribute.IsDefined(p, typeof(EditableAttribute))).ToArray();

            properties.ForEach(_ => Debug.WriteLine(_));
          
            int h = 0;
            
            foreach(var prop in properties)
            {
                var row = new PropertyRow();
                Controls.Add(row);
                row.Dock = DockStyle.Top;
                Control editor = CreateEditor(prop, out bool isTimeDependent);                         
                row.SetEditor(prop.Name, editor, isTimeDependent);          
                h += row.Height;
            }

            Height = h;            
        }

        private Control CreateEditor(PropertyInfo prop, out bool isTimeDependent)
        {
            Control editor = null;
            isTimeDependent = false;

            var targetType = prop.PropertyType;

            if (prop.PropertyType.IsGenericConstruct(typeof(TimeDependentValue<>)))
            {
                targetType = prop.PropertyType.GetGenericArguments()[0];
                isTimeDependent = true;
            }
            if (Reflection.DefaultEditors.TryGetValue(targetType, out Type editorType))
            {
                editor = Activator.CreateInstance(editorType) as Control;
            }
            if (editor == null) return null;

            editor.Tag = prop;
            if (editor is IPropertyEditorControl propEditorControl)
            {
                if (!isTimeDependent)
                {
                    propEditorControl.ObjectPropertyValue = prop.GetValue(Target);
                    propEditorControl.ObjectPropertyValueChanged += PropEditorControl_ObjectPropertyValueChanged;
                }
                else
                {
                    propEditorControl.ObjectPropertyValue = (prop.GetValue(Target) as ITimeDependentValue).CurrentValue;
                    propEditorControl.ObjectPropertyValueChanged += TimeDepPropEditorControl_ObjectPropertyValueChanged;
                }
                Editors.Add(propEditorControl);
            }


            return editor;
        }

        private void TimeDepPropEditorControl_ObjectPropertyValueChanged(object sender, EventArgs e)
        {
            
        }

        private void KeyframesViewLabel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PropEditorControl_ObjectPropertyValueChanged(object sender, EventArgs e)
        {
            var prop = (sender as Control).Tag as PropertyInfo;
            var val = (sender as IPropertyEditorControl).ObjectPropertyValue;
            prop.SetValue(Target, val);
            ObjectPropertyChanged?.Invoke(this, prop);
        }

        public event EventHandler<PropertyInfo> ObjectPropertyChanged;
    }
}
