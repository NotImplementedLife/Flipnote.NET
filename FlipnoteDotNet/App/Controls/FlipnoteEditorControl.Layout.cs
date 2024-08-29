using FlipnoteDotNet.Canvas;
using FlipnoteDotNet.PropertyEditor;
using FlipnoteDotNet.Utils.GUI;

namespace FlipnoteDotNet.App.Controls
{
    public partial class FlipnoteEditorControl
    {
        private SplitContainer RightPanel = new SplitContainer
        {
            Orientation = Orientation.Horizontal,
            IsSplitterFixed = false,            
        };

        private ComponentsListView ComponentsListView = new ComponentsListView
        {
            Dock = DockStyle.Fill
        };
        
        private ColumnsSplitter ColumnsSplitter = new ColumnsSplitter(new[]
        {
            DisplayLength.Proportional(1), DisplayLength.Proportional(2.5f), DisplayLength.Proportional(1)
        });

        private Panel MidContainer = new Panel { };

        private CanvasControl CanvasControl = new CanvasControl
        {
            Dock = DockStyle.Fill,
            AllowDrop = true
        };

        private FramesViewer FramesViewer = new FramesViewer
        {
            Dock = DockStyle.Bottom,
            Height = 100
        };

        private AssetsListView AssetsListView = new AssetsListView
        {
            Dock = DockStyle.Fill
        };

        private PropertyEditorControl PropertyEditor = new PropertyEditorControl
        {
            Dock = DockStyle.Fill
        };

        private ToolStrip AssetsToolStrip = new ToolStrip
        {
            ImageScalingSize = new Size(20, 20),
            GripStyle = ToolStripGripStyle.Hidden,
            Dock = DockStyle.Top
        };

        private ToolStrip ComponentsToolStrip = new ToolStrip
        {
            ImageScalingSize = new Size(20, 20),
            GripStyle = ToolStripGripStyle.Hidden,
            Dock = DockStyle.Top
        };

        private ToolStripButton AssetAddButton = CreateToolStripButton(Properties.Resources.ic_new_asset, "Add new asset");
        private ToolStripButton AssetRemoveButton = CreateToolStripButton(Properties.Resources.ic_remove_asset, "Remove asset");

        private ToolStripButton LayerMoveUpButton = CreateToolStripButton(Properties.Resources.ic_layer_move_up, "Move up");
        private ToolStripButton LayerMoveDownButton = CreateToolStripButton(Properties.Resources.ic_layer_move_down, "Move down");
        private ToolStripButton LayerRemoveButton = CreateToolStripButton(Properties.Resources.ic_layer_remove, "Remove layer");

        private static ToolStripButton CreateToolStripButton(Bitmap icon, string caption)
        {
            var button = new ToolStripButton
            {
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = icon,
                ImageTransparentColor = Color.Magenta,
                Name = caption + "Button",
                Text = caption,
                Size = new Size(24, 24)
            };
            return button;
        }

        private void InitializeLayout()
        {
            ComponentsToolStrip.Items.AddRange(new[] { LayerMoveUpButton, LayerMoveDownButton, LayerRemoveButton });

            RightPanel.Panel1.Controls.Add(ComponentsListView);
            RightPanel.Panel1.Controls.Add(ComponentsToolStrip);
            RightPanel.Panel2.Controls.Add(PropertyEditor);

            MidContainer.Controls.Add(CanvasControl);
            MidContainer.Controls.Add(FramesViewer);

            AssetsToolStrip.Items.AddRange(new[] { AssetAddButton, AssetRemoveButton });                        

            var assetsTab = new TabPage("Assets") { BackColor = Color.White, Width = 100, Height = 100 };
            assetsTab.Controls.Add(AssetsListView);
            assetsTab.Controls.Add(AssetsToolStrip);                       
            LeftTabControl.TabPages.Add(assetsTab);

            Controls.Add(LeftTabControl);
            Controls.Add(MidContainer);
            Controls.Add(RightPanel);

            ColumnsSplitter.Attach(this);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}
