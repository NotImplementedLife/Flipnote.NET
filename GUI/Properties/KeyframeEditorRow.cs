using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Diagnostics;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties
{
    public partial class KeyFrameEditorRow : UserControl
    {
        public KeyFrameEditorRow()
        {
            InitializeComponent();
            RemoveButtonTooltip.SetToolTip(RemoveButton, "Remove keyframe");
        }

        public int FrameIndex
        {
            set => FrameNoLabel.Text = (value + 1).ToString();
        }


        public static KeyFrameEditorRow CreateFromValueTransformer(IValueTransformer t, int timestamp)
        {
            Label createLabel(string text) => new Label
            {
                Anchor = AnchorStyles.None,
                Text = text,
                AutoSize = true
            };

            var row = new KeyFrameEditorRow();
            row.FrameIndex = timestamp;

            var editorWildcard = t.GetType().GetCustomAttribute<EditorWildcardAttribute>()?.Text ?? "<no editor>";
            var words = Regex.Split(editorWildcard, @"(%\d+)");
            var panel = new FlowLayoutPanel();

            panel.SuspendLayout();
            foreach(var word in words)
            {
                if (word.StartsWith("%"))
                {
                    var prop = t.GetType().GetProperties()
                        .Where(_ => _.GetCustomAttribute<ParameterWildcardAttribute>().Name == word).FirstOrDefault();
                    if (prop == null)
                    {
                        panel.Controls.Add(createLabel($"[No property with wildcard {word}]"));
                        continue;
                    }                    

                    if (!Reflection.DefaultEditors.TryGetValue(prop.PropertyType, out var editorType))
                    {
                        panel.Controls.Add(createLabel($"{t.GetType()}"));
                        panel.Controls.Add(createLabel($"[No editor for: {prop.PropertyType.Name}]"));
                        continue;
                    }

                    var editor = Activator.CreateInstance(editorType) as Control;
                    editor.Anchor = AnchorStyles.None;
                    editor.Tag = t;

                    if(editor is IPropertyEditorControl pec)
                    {
                        pec.Property = prop;

                        Debug.WriteLine(t);
                        Debug.WriteLine(prop.GetValue(t));
                        pec.ObjectPropertyValue = prop.GetValue(t);
                        pec.ObjectPropertyValueChanged += Pec_ObjectPropertyValueChanged;
                    }

                    panel.Controls.Add(editor);
                }
                else
                {
                    panel.Controls.Add(createLabel(word));
                }
            }
            panel.Location = new System.Drawing.Point(0, 0);
            panel.Dock = DockStyle.Top;
            panel.AutoSize = true;
            panel.ResumeLayout();
            panel.Padding = new Padding(0, 0, 0, 3);

            panel.Resize += (o, e) =>
            {
                row.EditorPanel.Height = (o as Control).Height + 5;
            };
            row.EditorPanel.Controls.Add(panel);  
                        

            return row;
        }

        private static void Pec_ObjectPropertyValueChanged(object sender, EventArgs e)
        {
            var target = (sender as Control).Tag;
            var pec = sender as IPropertyEditorControl;
            pec.Property.SetValue(target, pec.ObjectPropertyValue);
        }
    }
}
