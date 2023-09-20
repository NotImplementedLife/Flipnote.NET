using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.GUI.Properties.EditorFields;
using PPMLib.Rendering;
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
