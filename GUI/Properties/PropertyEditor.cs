using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
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

            int labelWidth = 75;
            int h = 0;
            
            foreach(var prop in properties)
            {
                var label = new Label
                {
                    Text = prop.Name,
                    AutoSize = false,
                    Width = labelWidth,
                    Height = 25,
                    AutoEllipsis = true,
                    TextAlign = System.Drawing.ContentAlignment.MiddleRight
                };

                Control editor = null;

                if (Reflection.DefaultEditors.TryGetValue(prop.PropertyType, out Type editorType)) 
                {
                    editor = Activator.CreateInstance(editorType) as Control;
                }

                var row = new Panel();
                row.Controls.Add(label);

                row.Padding = new Padding(3);

                if(editor!=null)
                {                                        
                    editor.Tag = prop;

                    label.Top = (editor.Height - label.Height) / 2;

                    row.Controls.Add(editor);

                    editor.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    editor.Left = labelWidth;
                    editor.Top = 0;                    
                    editor.Height = Math.Max(label.Height, editor.PreferredSize.Height);

                    Debug.WriteLine($"E : {editor.Width}: {editor.Right} : {editor.Left}");

                    if (editor is IPropertyEditorControl propEditorControl)
                    {
                        propEditorControl.ObjectPropertyValue = prop.GetValue(Target);
                        propEditorControl.ObjectPropertyValueChanged += PropEditorControl_ObjectPropertyValueChanged;
                        Editors.Add(propEditorControl);
                    }
                }
                row.AutoSize = true;
                row.Dock = DockStyle.Top;
                Controls.Add(row);
                if (editor != null)
                {
                    editor.Width = row.Width - labelWidth - 3;
                    editor.Anchor |= AnchorStyles.Right;
                }
                h += row.Height;
            }

            Height = h;
            //ResumeLayout(true);
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
