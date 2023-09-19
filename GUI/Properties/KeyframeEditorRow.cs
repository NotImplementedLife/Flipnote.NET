using FlipnoteDotNet.Attributes;
using static FlipnoteDotNet.Constants;
using FlipnoteDotNet.Utils.Temporal;
using System;
using System.Linq;
using System.Reflection;
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
        
        public IValueTransformer Transformer { get; set; }

        public event EventHandler<IValueTransformer> TransformerChanged;
        public event EventHandler<IValueTransformer> RemoveButtonClicked;


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
            row.Transformer = t;

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
                    editor.Tag = new TagValue(t, row, prop);                    

                    if(editor is IPropertyEditorControl pec)
                    {
                        pec.Property = prop;                        
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
            var tv = (sender as Control).Tag as TagValue;
            var pec = sender as IPropertyEditorControl;
            tv.TranformerProperty.SetValue(tv.Target, pec.ObjectPropertyValue);
            tv.Editor.TransformerChanged?.Invoke(tv.Editor, tv.Target);
            
        }

        class TagValue
        {
            public IValueTransformer Target { get; }
            public KeyFrameEditorRow Editor { get; }
            public PropertyInfo TranformerProperty { get; }

            public TagValue(IValueTransformer target, KeyFrameEditorRow editor, PropertyInfo tranformerProperty)
            {
                Target = target;
                Editor = editor;
                TranformerProperty = tranformerProperty;
            }
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            RemoveButtonClicked?.Invoke(this, Transformer);            
        }
    }
}
