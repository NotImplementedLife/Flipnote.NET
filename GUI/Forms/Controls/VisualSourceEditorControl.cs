using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.GUI.Controls.Primitives;
using FlipnoteDotNet.Utils.Paint;
using PPMLib.Data;
using PPMLib.Rendering;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FlipnoteDotNet.GUI.Forms.Controls
{
    public partial class VisualSourceEditorControl : UserControl
    {
        public VisualSourceEditorControl()
        {
            InitializeComponent();
            PaintContext.PenValue = 1;
            SetCanvasColors();

            if (Constants.IsDesignerMode) 
                return;

            foreach (var (ToolType, ToolName, ToolIcon, PntCtxEditorType) in Constants.Reflection.PaintTools)
            {
                IToolStripCheckButton button = null;
                if (PntCtxEditorType == null)
                {
                    button = new ToolStripCheckButton
                    {
                        CheckOnClick = true,                                                
                    };                    
                }
                else
                {
                    button = new ToolStripSplitCheckButton
                    {
                        CheckOnClick = true
                    };

                    var pntCtxEditor = Activator.CreateInstance(PntCtxEditorType) as IPaintContextEditor;
                    var btn = button as ToolStripSplitCheckButton;
                    var container = new PoperContainer(new PaintContextEditorPopedContainer(pntCtxEditor, PaintContext));
                    container.Closed += (o, ev) =>
                    {
                        SetCanvasColors();
                    };

                    btn.DropDown = container;
                    
                }

                button.Image = ToolIcon;
                button.ToolTipText = ToolName;                
                button.Tag = ToolType;
                ToolStrip.Items.Add(button as ToolStripItem);

                button.Click += (sender, args) =>
                {
                    if (ActiveToolButton != null) ActiveToolButton.Checked = false;
                    ActiveToolButton = null;
                    Canvas.AttachOperation(null);

                    var btn = sender as IToolStripCheckButton;                                        
                    if (btn.Checked)
                    {
                        ActiveToolButton = btn;
                        ActiveToolButton.Checked = true;

                        var tool = Activator.CreateInstance(btn.Tag as Type) as IPaintTool;
                        var operation = tool.CreateOperation();
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

        private IToolStripCheckButton ActiveToolButton;

        PaintContext PaintContext = new PaintContext();

        private void SetCanvasColors()
        {
            Canvas.SetColors(PaintContext.Pen1.ToColor(FlipnotePaperColor.White), PaintContext.Pen2.ToColor(FlipnotePaperColor.White));
        }

        private void UpdateVisuals()
        {
            _VisualSource = _VisualSource ?? new FlipnoteVisualSource(8, 8);
            Canvas.LoadFromFlipnoteVisualSoruce(_VisualSource);
        }

        private FlipnoteVisualSource _VisualSource = new FlipnoteVisualSource(8, 8);

        public FlipnoteVisualSource VisualSource
        {
            get => _VisualSource;
            set
            {
                _VisualSource = value;
                UpdateVisuals();
            }
        }

        public FlipnoteVisualSource GetVisualSourceFromCanvas() => Canvas.ToFlipnoteVisualSource();
    }
}
