using PPMLib.Rendering;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms
{
    [PropertyEditorControl(typeof(FlipnoteVisualSource))]
    internal class VisualSourceEditorForm : Form, IObjectHolderDialog
    {
        public object ObjectValue { get; set; }

        /*
       
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

        */
    }
}
