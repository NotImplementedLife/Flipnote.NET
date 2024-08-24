

using FlipnoteDotNet.App.Forms;
using FlipnoteDotNet.Drawing.Renderers;
using System.ComponentModel;
using System.Diagnostics;

namespace FlipnoteDotNet.App.Controls
{
    public class VisualRendererBox : ComboBox
    {
        private readonly string[] Options = new[] { "Software", "Hardware accelerated" };

        public VisualRendererBox() 
        {
            Items.AddRange(Options);
            InitializeComponent();            
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ObjectCollection Items => base.Items;

        public void InitializeComponent()
        {
            SuspendLayout();
            DropDownStyle = ComboBoxStyle.DropDownList;            
            SelectedIndex = 0;
            ResumeLayout();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            switch (SelectedIndex)
            {
                case 0:
                    {
                        if (Config.VisualRenderer is GDIVisualRenderer) return;
                        PendingForm.ShowIfNecessary(this, () => Config.VisualRenderer = new GDIVisualRenderer());
                        return;
                    }
                case 1:
                    {
                        if (Config.VisualRenderer is CLVisualRenderer) return;
                        PendingForm.ShowIfNecessary(this, () => Config.VisualRenderer = new CLVisualRenderer());
                        return;
                    }
            }            
        }
    }
}
