using static FlipnoteDotNet.Constants;
using FlipnoteDotNet.Utils.Temporal;
using System;
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
            row.TransformerChanged += Row_TransformerChanged;
            row.RemoveButtonClicked += Row_RemoveButtonClicked;
            Controls.Add(row);
            row.BringToFront();
        }

        public event EventHandler<IValueTransformer> TransformerChanged;
        public event EventHandler<IValueTransformer> TransformerRemoved;

        private void Row_RemoveButtonClicked(object sender, IValueTransformer e)
        {
            Controls.Remove(sender as Control);
            TransformerRemoved?.Invoke(this, e);
        }        

        private void Row_TransformerChanged(object sender, IValueTransformer e)
        {
            TransformerChanged?.Invoke(this, e);
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
