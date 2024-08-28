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

        private Button AssetImportButton = new Button
        {
            Text = "Import Asset",
            Top = 10,
            Left = 10,
            BackColor = SystemColors.Control
        };

        private PropertyEditorControl PropertyEditor = new PropertyEditorControl
        {
            Dock = DockStyle.Fill
        };

        private void InitializeLayout()
        {
            RightPanel.Panel1.Controls.Add(ComponentsListView);
            RightPanel.Panel2.Controls.Add(PropertyEditor);            

            MidContainer.Controls.Add(CanvasControl);
            MidContainer.Controls.Add(FramesViewer);

            var assetsTab = new TabPage("Assets") { BackColor = Color.White, Width = 100, Height = 100 };
            AssetImportButton.PerformLayout();
            AssetsListView.Top = AssetImportButton.Top + AssetImportButton.Height + 5;
            AssetsListView.Width = 100 - 2 * AssetsListView.Left;
            AssetsListView.Height = 100 - AssetsListView.Top - 10;
            AssetsListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            assetsTab.Controls.Add(AssetImportButton);
            assetsTab.Controls.Add(AssetsListView);
            assetsTab.PerformLayout();
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
