using FlipnoteDotNet.Constants;
using FlipnoteDotNet.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties
{
    public partial class KeyFramesEditor : UserControl
    {
        public KeyFramesEditor()
        {
            InitializeComponent();
        }

        public PropertyInfo Property { get; set; }

        public void ClearEditors()
        {
            Controls.Clear();
        }

        public void AddEditor(KeyFrameEditorRow row)
        {
            row.Dock = DockStyle.Top;
            Controls.Add(row);
            row.BringToFront();
        }        

        public void AddCaption(string text)
        {
            var label = new Label
            {
                Text = text,
                BackColor = Colors.FlipnoteThemeMainColor,
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                AutoSize = false,
                Height = 25,
                TextAlign = ContentAlignment.MiddleLeft
            };
            Controls.Add(label);
            label.BringToFront();
        }
    }
}
