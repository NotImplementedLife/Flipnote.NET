using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Canvas;

namespace FlipnoteDotNet.App.Controls
{
    public partial class FlipnoteEditorControl : Panel
    {
        UndoStack UndoStack;
        FramesManager FramesManager;

        private readonly TabControl LeftTabControl = new TabControl();

        public FlipnoteEditorControl()
        {

        }

        public void Initialize(AppState appState)
        {
            UndoStack = appState.UndoStack;
            FramesManager = appState.FramesManager;
            FramesManager.CanvasModel = CanvasControl.CanvasModel;

            FramesManager.CurrentFrameChanged += FramesManager_CurrentFrameChanged;

            FramesManager.AddNewFrame();
            FramesManager.SetCurrentFrame(0);

            InitializeLayout();
            InitializeAssets(appState.AssetsService);
            InitializeFrames(appState.FlipnoteEditorService);            

            FramesViewer.FramesManager = FramesManager;
            FramesManager.RequestThumbnailRedraw(FramesManager.GetCurrentFrame());            
        }

        private void FramesManager_CurrentFrameChanged(object sender, EventArgs e)
        {
            ComponentsListView.SetData(FramesManager.GetCurrentFrame().Components);
        }
    }
}
