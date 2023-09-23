using FlipnoteDotNet.Attributes;
using static FlipnoteDotNet.Constants;
using FlipnoteDotNet.Data;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Properties.EditorFields;
using FlipnoteDotNet.Properties;
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
using Brushes = System.Drawing.Brushes;

namespace FlipnoteDotNet.GUI.Properties
{
    public partial class PropertyEditor : UserControl
    {
        public PropertyEditor()
        {
            InitializeComponent();
            this.EnableDoubleBuffer();
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
                if (_Target == value)
                {
                    Debug.WriteLine("ERE?");
                    ReloadPropertyRows();
                    return;
                }

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

        public void ReloadValues()
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

        public List<PropertyEditorData> PropertyEditorRows { get; } = new List<PropertyEditorData>();

        private int RowPadding = 3;

        private void ReloadPropertyRows()
        {                                  
            Editors.ForEach(e => e.ObjectPropertyValueChanged -= PropEditorControl_ObjectPropertyValueChanged);
            Editors.Clear();
            Controls.Clear();
            PropertyEditorRows.Clear();

            if (Target == null) return;
            var properties = Target.GetType().GetAllPublicProperties()
                .Where(p => Attribute.IsDefined(p, typeof(EditableAttribute))).ToArray();

            properties.ForEach(_ => Debug.WriteLine(_));

            int h = RowPadding;
            
            foreach(var prop in properties)
            {              
                Control editor = CreateEditor(prop, out bool isTimeDependent, out var _);               
                var ped = new PropertyEditorData(prop, prop.Name, editor, isTimeDependent);
                ped.Y = h;

                if (isTimeDependent)
                {
                    ped.KeyframesButtonClick += PropertyRow_KeyframesButtonClick;
                    ped.EffectsButtonClick += PropertyRow_EffectsButtonClick;
                }

                PropertyEditorRows.Add(ped);

                if (editor is IPropertyEditorControl propEditor)
                    Editors.Add(propEditor);

                h += Math.Max(25, editor.Height) + 2 * RowPadding;
            }
            Height = h;

            SuspendLayout();

            foreach (var row in PropertyEditorRows) 
            {
                row.Control.Left = 75 + 3;
                row.Control.Top = row.Y;
                row.Control.Width = Math.Max(1, Width - row.Control.Left - 60);
                row.Control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                Controls.Add(row.Control);
            }

            ResumeLayout(true);
            Invalidate();
            ReloadValues();            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var format = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };


            var cpos = PointToClient(Cursor.Position);
            var hoverBrush = Color.Gray.Alpha(64).GetBrush();
            var w = Resources.ic_keyframes.Width;
            int d = (24 - w) / 2;
            foreach (var row in PropertyEditorRows)
            {                
                var bounds = new Rectangle(0, row.Y, 75, 25);
                e.Graphics.DrawString(row.Label, Font, Brushes.Black, bounds, format);

                var cy = row.Y + (row.Control.Height - 24) / 2;

                if(row.IsTimeDependent)
                {
                    var b1 = new Rectangle(row.Control.Right + 5, cy, 24, 24);
                    var b2 = new Rectangle(row.Control.Right + 5 + 24 + 5, cy, 24, 24);

                    if (b1.Contains(cpos)) e.Graphics.FillRectangle(hoverBrush, b1);
                    else if (b2.Contains(cpos)) e.Graphics.FillRectangle(hoverBrush, b2);

                    e.Graphics.DrawImageUnscaled(Resources.ic_keyframes, b1.X + d, b1.Y + d);
                    e.Graphics.DrawImageUnscaled(Resources.ic_effects, b2.X + d, b2.Y + d);
                }
            }
        }

        private int PropsYLowerBound(int y)
        {                        
            int low = 0;
            int high = PropertyEditorRows.Count;            
            while (low < high)
            {
                int mid = low + (high - low) / 2;                
                if (y <= PropertyEditorRows[mid].Y)                 
                    high = mid;                             
                else                
                    low = mid + 1;                
            }
            if (low < PropertyEditorRows.Count && PropertyEditorRows[low].Y < y)
                low++;            
            return low;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            int cnt = PropsYLowerBound(e.Y);
            if (cnt == 0) return;

            int rowId = cnt - 1;
            var row = PropertyEditorRows[rowId];            

            var cy = row.Y + (row.Control.Height - 24) / 2;
            var b1 = new Rectangle(row.Control.Right + 5, cy, 24, 24);
            var b2 = new Rectangle(row.Control.Right + 5 + 24 + 5, cy, 24, 24);

            if (b1.Contains(e.Location))
                row.TriggerKeyframesButtonClick();
            else if (b2.Contains(e.Location))
                row.TriggerEffectsButtonClick();            

            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            Invalidate();
        }

        private Control CreateEditor(PropertyInfo prop, out bool isTimeDependent, out Type editType)
        {
            Control editor = null;
            isTimeDependent = false;

            var targetType = prop.PropertyType;            

            if (prop.PropertyType.IsGenericConstruct(typeof(TimeDependentValue<>)))
            {
                targetType = prop.PropertyType.GetGenericArguments()[0];                
                isTimeDependent = true;
            }
            editType = targetType;

            var propertyEditorControlType = prop.GetCustomAttribute<PropertyEditorControlAttribute>()?.Type;
            
            Control CreateEditorControlFromType(Type type)
            {                
                if (type.GetInterfaces().Contains(typeof(IObjectHolderDialog)))
                    return new FormBasedEditor(type);
                return Activator.CreateInstance(type) as Control;
            }

            if (propertyEditorControlType != null)
            {                                
                editor = CreateEditorControlFromType(propertyEditorControlType);
            }
            else if(targetType.IsEnum)
            {
                var edType = typeof(EnumEditorField<>).MakeGenericType(targetType);
                editor = CreateEditorControlFromType(edType);
            }
            else if (Reflection.DefaultEditors.TryGetValue(targetType, out Type editorType))
            {
                editor = CreateEditorControlFromType(editorType);
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
            tdv.PutTransformer(ConstantValueTransformerFactory
                .MakeFromType(innerPropType, val), (Target as ITemporalContext).CurrentTimestamp);
            //tdv.PutTransformer(new ConstantValueTransformer(val), (Target as ITemporalContext).CurrentTimestamp);
            tdv.UpdateTransformations();
            tdv.UpdateTimestamps();

            if (KeyFramesEditor.Property == prop) 
            {
                RefreshKeyFramesEditor();
            }

            ObjectPropertyChanged?.Invoke(this, prop);
        }

        private void PropertyRow_EffectsButtonClick(object sender, EventArgs e)
        {

        }

        private void PropertyRow_KeyframesButtonClick(object sender, EventArgs e)
        {            
            var prop = (sender as PropertyEditorData).Property;
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
