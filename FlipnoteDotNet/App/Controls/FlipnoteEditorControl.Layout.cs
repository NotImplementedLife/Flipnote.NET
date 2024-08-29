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
            Top = 15,
            Left = 10,
        };   

        private PropertyEditorControl PropertyEditor = new PropertyEditorControl
        {
            Dock = DockStyle.Fill
        };

        private ToolStrip AssetsToolStrip = new ToolStrip
        {
            ImageScalingSize = new Size(20, 20),
            GripStyle = ToolStripGripStyle.Hidden
        };

        private ToolStripButton AssetAddButton = CreateToolStripButton(Properties.Resources.ic_new_asset, "Add new asset");
        private ToolStripButton AssetRemoveButton = CreateToolStripButton(Properties.Resources.ic_remove_asset, "Remove asset");

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
            RightPanel.Panel1.Controls.Add(ComponentsListView);
            RightPanel.Panel2.Controls.Add(PropertyEditor);            

            MidContainer.Controls.Add(CanvasControl);
            MidContainer.Controls.Add(FramesViewer);

            AssetsToolStrip.Items.AddRange(new[] { AssetAddButton, AssetRemoveButton });
            AssetsToolStrip.Dock = DockStyle.Top;

            AssetsListView.Dock = DockStyle.Fill;

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
