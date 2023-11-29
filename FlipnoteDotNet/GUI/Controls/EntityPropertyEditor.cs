using FlipnoteDotNet.Commons;
using FlipnoteDotNet.Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Controls
{
    public class EntityPropertyEditor : Panel
    {
        public EntityPropertyEditor()
        {
            DoubleBuffered = true;
            AutoScroll = true;
            BackColor = Color.White;
        }        

        private class Editor
        {
            public PropertyInfo Property;
            public string PropertyName;
            public Control Control;
            public int Y;
            public int NameHeight;
            public Editor(string propertyName, Control control, int y)
            {
                PropertyName = propertyName;
                Control = control;
                Y = y;
            }
        }

        private readonly List<Editor> Editors = new List<Editor>();
        private int LabelsWidth = 0;


        Type EntityType = null;
        private void CreateEditors(Type entityType)
        {
            if (EntityType == entityType)
                return;
            EntityType = entityType;
            DetachFieldChangedEvents();
            Editors.Clear();
            if (entityType == null)
            {
                RefreshControls();                
                return;
            }
            
            LabelsWidth = 0;
            int y = 3;            

            var properties = entityType.GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetCustomAttribute<HiddenAttribute>() != null)
                    continue;

                var name = properties[i].Name;

                var measure = TextRenderer.MeasureText(name, Font);                                 
                LabelsWidth = Math.Max(LabelsWidth, measure.Width);

                var fieldEditorAttribute = properties[i].GetCustomAttribute<FieldEditorAttribute>();                

                var control = fieldEditorAttribute != null
                    ? Activator.CreateInstance(fieldEditorAttribute.FieldType) as Control
                    : PropertyEditorControls.CreateField(properties[i].PropertyType);                

                if(control is IPropertyEditorControl pec)                
                    pec.Property = properties[i];                                    

                Editors.Add(new Editor(name, control, y) { NameHeight = measure.Height, Property = properties[i] });
                y += (control?.Height ?? measure.Height) + 6;
            }
            LabelsWidth += 10;
            Editors.Sort((e1, e2) => e1.PropertyName.CompareTo(e2.PropertyName));
            
            RefreshControls();
        }

        private void Field_ObjectPropertyValueChanged(object o, EventArgs e)
        {            
            if (!(o is IPropertyEditorControl pec)) return; 
            pec.Property.SetValue(pTargetEntity.Entity, pec.ObjectPropertyValue);
            PropertyValueChanged?.Invoke(this, pec.Property, pec.ObjectPropertyValue);            
        }

        void AttachFieldChangedEvents()
        {
            foreach (var ed in Editors)
                if (ed.Control is IPropertyEditorControl pec)
                    pec.ObjectPropertyValueChanged += Field_ObjectPropertyValueChanged;
        }

        void DetachFieldChangedEvents()
        {
            foreach (var ed in Editors)
                if (ed.Control is IPropertyEditorControl pec)
                    pec.ObjectPropertyValueChanged -= Field_ObjectPropertyValueChanged;
        }

        public delegate void OnPropertyValueChanged(object sender, PropertyInfo property, object newValue);
        public event OnPropertyValueChanged PropertyValueChanged;

        private void RefreshControls()
        {
            SuspendLayout();
            Controls.Clear();
            
            foreach (var e in Editors)
            {
                if (e.Control == null) continue;
                e.Control.Location = new Point(LabelsWidth + 5, e.Y);
                e.Control.Width = Width - 5 - e.Control.Left;
                e.Control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                Controls.Add(e.Control);                
            }
            ResumeLayout();
            Invalidate();
        }

        private IEntityReference<Entity> pTargetEntity = null;

        public IEntityReference<Entity> TargetEntity => pTargetEntity;

        public void SetEntity(IEntityReference<Entity> eRef, bool commit = true)
        {
            Debug.WriteLine("SetEntity");
            //if (commit) pTargetEntity?.Commit();
            pTargetEntity = eRef;            
            CreateEditors(pTargetEntity?.Entity?.GetType());

            DetachFieldChangedEvents();
            foreach (var editor in Editors)
            {
                var value = editor.Property.GetValue(eRef.Entity);
                (editor.Control as IPropertyEditorControl).ObjectPropertyValue = value;
            }
            AttachFieldChangedEvents();
        }            

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);            

            foreach (var ed in Editors)
            {
                var p = new Point(5, ed.Y + AutoScrollPosition.Y + ((ed.Control?.Height ?? ed.NameHeight) - ed.NameHeight) / 2);
                TextRenderer.DrawText(e.Graphics, ed.PropertyName, Font, p, ForeColor);
            }
        }
    }
}
