using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Properties
{
    public partial class PropertyRow : UserControl
    {
        public PropertyRow()
        {
            InitializeComponent();

            EffectsTooltip.SetToolTip(EffectsButton, "Effects");
            KeyframesTooltip.SetToolTip(KeyframesButton, "Keyframes");            
        }

        public void SetEditor(string caption, Control control, bool isTimeDependent)
        {
            Label.Text = caption;

            if(!isTimeDependent)
            {
                Controls.Remove(KeyframesButton);
                Controls.Remove(EffectsButton);
            }

            if (control != null)
            {
                EditorPanel.Controls.Add(control);
                Height = Math.Max(control.Height, Label.Height);
                control.Width = EditorPanel.Width;
                control.Top = (EditorPanel.Height - control.Height) / 2;
                control.Anchor |= AnchorStyles.Right;
            }
            else
            {
                Height = 28;
            }
        }
    }
}
