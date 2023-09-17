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
        private ToolStripButton ActiveToolButton;

        PaintContext PaintContext = new PaintContext();

        public VisualSourceEditorForm()
        {
            InitializeComponent();
            PaintContext.PenValue = 1;

            foreach(var (ToolType, ToolName, ToolIcon) in Constants.Reflection.PaintTools)
            {
                var button = new ToolStripButton();
                button.Image = ToolIcon;
                button.ToolTipText = ToolName;
                button.CheckOnClick = true;
                button.Tag = ToolType;
                ToolStrip.Items.Add(button);

                button.Click += (sender, args) =>
                {
                    var btn = sender as ToolStripButton;
                    if (!btn.Checked)
                    {
                        ActiveToolButton = null;
                        Canvas.AttachOperation(null);
                    }
                    else
                    {
                        ActiveToolButton = btn;
                        ActiveToolButton.Checked = true;

                        var tool = Activator.CreateInstance(btn.Tag as Type) as IPaintTool;
                        var operation = tool.InitOperation();
                        operation.PaintContext = PaintContext;
                        Canvas.AttachOperation(operation);
                    }                    
                };
            }
            

            UpdateVisuals();
            Canvas.CanvasViewLocation = new Point(0, 0);
            Canvas.CanvasViewScaleFactor = 700;

            Canvas.DisableMouseGestures();
            Canvas.EnableMouseGestures();
        }        

        private void UpdateVisuals()
        {
            VisualSource = VisualSource ?? new FlipnoteVisualSource(8, 8);
            Canvas.LoadFromFlipnoteVisualSoruce(VisualSource);            
        }      

        FlipnoteVisualSource VisualSource { get; set; }

        public object ObjectValue 
        { 
            get=>VisualSource;
            set
            {
                VisualSource = value as FlipnoteVisualSource;
                UpdateVisuals();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ObjectValue = Canvas.ToFlipnoteVisualSource();
            DialogResult = DialogResult.OK;
        }


    }
}
