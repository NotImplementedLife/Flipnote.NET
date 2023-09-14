using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils.Temporal;
using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

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
                if (_Target is ITemporalContext tctx)                 
                    tctx.CurrentTimestampChanged -= TemporalTarget_CurrentTimestampChanged;                
                _Target = value;
                if (_Target is ITemporalContext newTctx)
                    newTctx.CurrentTimestampChanged += TemporalTarget_CurrentTimestampChanged;                

                ReloadPropertyRows();
                TargetChanged?.Invoke(this, new EventArgs());
            }
        }

        private void TemporalTarget_CurrentTimestampChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("TemporalTarget_CurrentTimestampChanged");
            ReloadValues();
        }

        public event EventHandler TargetChanged;

        private List<IPropertyEditorControl> Editors { get; } = new List<IPropertyEditorControl>();        

        private void ReloadValues()
        {
            foreach(var editor in Editors)
            {
                if(!editor.IsTimeDependent)
                {
                    editor.ObjectPropertyValueChanged -= PropEditorControl_ObjectPropertyValueChanged;
                    editor.ObjectPropertyValue = editor.Property.GetValue(Target);
                    editor.ObjectPropertyValueChanged += PropEditorControl_ObjectPropertyValueChanged;
                }
                else
                {
                    editor.ObjectPropertyValueChanged -= TimeDepPropEditorControl_ObjectPropertyValueChanged;
                    editor.ObjectPropertyValue = (editor.Property.GetValue(Target) as ITimeDependentValue).CurrentValue;
                    editor.ObjectPropertyValueChanged += TimeDepPropEditorControl_ObjectPropertyValueChanged;
                }              
            }
        }

        private void ReloadPropertyRows()
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
                row.SetPropertyTags(prop);

                if (isTimeDependent)
                {
                    row.KeyframesButtonClick += PropertyRow_KeyframesButtonClick;
                    row.EffectsButtonClick += PropertyRow_EffectsButtonClick;
                }

                h += row.Height;
            }
            Height = h;
            ReloadValues();
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
                propEditorControl.Property = prop;
                propEditorControl.IsTimeDependent = isTimeDependent;
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
            var prop = (sender as Control).Tag as PropertyInfo;
            var val = (sender as IPropertyEditorControl).ObjectPropertyValue;           

            var tdv = prop.GetValue(Target) as ITimeDependentValue;
            tdv.PutTransformer(new ConstantValueTransformer(val), (Target as ITemporalContext).CurrentTimestamp);
            tdv.UpdateTransformations();
            tdv.UpdateTimestamps();       
        }

        private void PropertyRow_EffectsButtonClick(object sender, EventArgs e)
        {

        }

        private void PropertyRow_KeyframesButtonClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            MessageBox.Show($"{(control.Tag as PropertyInfo).Name}");
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
