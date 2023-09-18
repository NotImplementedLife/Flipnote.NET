using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Data.Drawing;
using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Canvas;
using FlipnoteDotNet.GUI.Canvas.Components;
using FlipnoteDotNet.GUI.Properties;
using FlipnoteDotNet.GUI.Properties.EditorFields;
using FlipnoteDotNet.Utils.Paint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    [PropertyEditorControl(typeof(FlipnoteVisualSource))]
    public partial class VisualSourceEditorForm : Form, IObjectHolderDialog
    {        
        public VisualSourceEditorForm()
        {
            InitializeComponent();                      
        }              

        public object ObjectValue 
        { 
            get=>VisualSourceEditorControl.VisualSource;
            set
            {
                VisualSourceEditorControl.VisualSource = value as FlipnoteVisualSource;                
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ObjectValue = VisualSourceEditorControl.GetVisualSourceFromCanvas();
            DialogResult = DialogResult.OK;
        }


    }
}
