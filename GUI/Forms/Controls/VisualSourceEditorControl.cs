using FlipnoteDotNet.Data.Drawing;
using FlipnoteDotNet.Utils.Paint;
using System;
using System.ComponentModel;
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

            if ((LicenseManager.UsageMode == LicenseUsageMode.Designtime))
                return;

            foreach (var (ToolType, ToolName, ToolIcon) in Constants.Reflection.PaintTools)
            {
                var button = new ToolStripButton();
                button.Image = ToolIcon;
                button.ToolTipText = ToolName;
                button.CheckOnClick = true;
                button.Tag = ToolType;
                ToolStrip.Items.Add(button);

                button.Click += (sender, args) =>
                {
                    if (ActiveToolButton != null) ActiveToolButton.Checked = false;
                    ActiveToolButton = null;
                    Canvas.AttachOperation(null);

                    var btn = sender as ToolStripButton;
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

        private ToolStripButton ActiveToolButton;

        PaintContext PaintContext = new PaintContext();

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
