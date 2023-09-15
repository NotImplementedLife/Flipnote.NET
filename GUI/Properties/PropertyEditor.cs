using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Properties.EditorFields;
using FlipnoteDotNet.Utils.Temporal;
using FlipnoteDotNet.Utils.Temporal.ValueTransformers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

        private KeyFramesEditor _KeyFramesEditor;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public KeyFramesEditor KeyFramesEditor 
        { 
            get=>_KeyFramesEditor;
            set
            {
                if (_KeyFramesEditor != null)
                {
                    _KeyFramesEditor.TransformerChanged -= KeyFramesEditor_TransformerChanged;
                    _KeyFramesEditor.TransformerRemoved -= KeyFramesEditor_TransformerRemoved;
                }
                if ((_KeyFramesEditor = value) != null) 
                {
                    _KeyFramesEditor.TransformerChanged += KeyFramesEditor_TransformerChanged;
                    _KeyFramesEditor.TransformerRemoved += KeyFramesEditor_TransformerRemoved;
                }
            }
        }

        private void KeyFramesEditor_TransformerRemoved(object sender, IValueTransformer e)
        {
            if (!(Target is AbstractTransformableTemporalContext ttctx)) return;
            var tdv = KeyFramesEditor.Property.GetValue(ttctx) as ITimeDependentValue;
            ttctx.RemoveTransformer(tdv, e);
            ttctx.UpdateTransformations(tdv);                        
            tdv.UpdateTimestamps();
            ReloadValues();
        }

        private void KeyFramesEditor_TransformerChanged(object sender, IValueTransformer e)
        {
            if (!(Target is AbstractTransformableTemporalContext ttctx)) return;
            var tdv = KeyFramesEditor.Property.GetValue(ttctx) as ITimeDependentValue;
            ttctx.UpdateTransformations(tdv);
            tdv.UpdateTimestamps();
            ReloadValues();

        }

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
                if(_Target is ITimeLocalizable tloc)
                    tloc.StartTimestampChanged += TemporalTarget_StartTimestampChanged;
                _Target = value;
                if (_Target is ITemporalContext newTctx)
                    newTctx.CurrentTimestampChanged += TemporalTarget_CurrentTimestampChanged;
                if (_Target is ITimeLocalizable newTloc)
                    newTloc.StartTimestampChanged += TemporalTarget_StartTimestampChanged;
                ReloadPropertyRows();
                TargetChanged?.Invoke(this, new EventArgs());
            }
        }

        private void TemporalTarget_StartTimestampChanged(object sender, PropertyChanedEventArgs<int> e)
        {            
            ReloadValues();
            KeyFramesEditor.ClearEditors();            
        }

        private void TemporalTarget_CurrentTimestampChanged(object sender, PropertyChanedEventArgs<int> e)
        {            
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

            SuspendLayout();
            foreach(var prop in properties)
            {
                var row = new PropertyRow();
                Controls.Add(row);
                row.Dock = DockStyle.Top;
                Control editor = CreateEditor(prop, out bool isTimeDependent);                         
                row.SetEditor(prop.Name, editor, isTimeDependent);
                row.SetPropertyTags(prop);
                row.BringToFront();

                if (isTimeDependent)
                {
                    row.KeyframesButtonClick += PropertyRow_KeyframesButtonClick;
                    row.EffectsButtonClick += PropertyRow_EffectsButtonClick;
                }

                h += row.Height;
            }
            Height = h;
            ResumeLayout(true);
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

            var propertyEditorControlType = prop.GetCustomAttribute<PropertyEditorControlAttribute>()?.Type;
                       
            if (propertyEditorControlType != null)
            {                
                editor = Activator.CreateInstance(propertyEditorControlType) as Control;
            }
            else if (Reflection.DefaultEditors.TryGetValue(targetType, out Type editorType))
            {
                editor = Activator.CreateInstance(editorType) as Control;
            }


            if (editor == null)
                editor = new DefaultNonEditable();
            
            editor.Tag = prop;
            if (editor is IPropertyEditorControl propEditorControl)
            {
                propEditorControl.KeyframesEditor = KeyFramesEditor;
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
            var innerPropType = prop.PropertyType.GetGenericArguments()[0];

            var tdv = prop.GetValue(Target) as ITimeDependentValue;
            tdv.PutTransformer(ConstantValueTransformer
                .MakeFromType(innerPropType, val), (Target as ITemporalContext).CurrentTimestamp);
            //tdv.PutTransformer(new ConstantValueTransformer(val), (Target as ITemporalContext).CurrentTimestamp);
            tdv.UpdateTransformations();
            tdv.UpdateTimestamps();

            if (KeyFramesEditor.Property == prop) 
            {
                RefreshKeyFramesEditor();
            }
        }

        private void PropertyRow_EffectsButtonClick(object sender, EventArgs e)
        {

        }

        private void PropertyRow_KeyframesButtonClick(object sender, EventArgs e)
        {
            var control = sender as Control;
            var prop = (control.Tag as PropertyInfo);

            KeyFramesEditor.Property = prop;
            RefreshKeyFramesEditor();

            KeyFramesButtonClick?.Invoke(this, new EventArgs());
        }

        public event EventHandler KeyFramesButtonClick;

        private void RefreshKeyFramesEditor()
        {
            KeyFramesEditor.SuspendLayout();
            KeyFramesEditor.ClearEditors();
            var ttctx = Target as AbstractTransformableTemporalContext;
            if (ttctx == null) goto __end;
            KeyFramesEditor.AddCaption("Property : " + KeyFramesEditor.Property?.Name);
            var tdv = KeyFramesEditor.Property?.GetValue(Target) as ITimeDependentValue;
            if (tdv == null) goto __end;
            
            foreach (var (transformer, timestamp) in ttctx.GetTransformers(tdv).OrderBy(_ => _.Timestamp)) 
            {
                KeyFramesEditor.AddEditor(KeyFrameEditorRow.CreateFromValueTransformer(transformer, timestamp));                
            }

            __end:
            KeyFramesEditor.ResumeLayout(true);           
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
