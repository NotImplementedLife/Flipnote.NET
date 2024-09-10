using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service;
using FlipnoteDotNet.Canvas;

namespace FlipnoteDotNet.App.Controls
{
    public partial class FlipnoteEditorControl : Panel
    {
        UndoStack UndoStack;
        FramesManager FramesManager;

        private readonly TabControl LeftTabControl = new TabControl();

        public CanvasModel CanvasModel => CanvasControl.CanvasModel;

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


        public void ChangeState(AppState appState)
        {
            FramesManager.CanvasModel = null;
            FramesManager.CurrentFrameChanged -= FramesManager_CurrentFrameChanged;
            FramesManager = null;
            FramesViewer.FramesManager = null;

            AssetsListView.ResetList();            
            
            AssetsService = appState.AssetsService;
            AssetsListView.TargetList = AssetsService.Assets;    

            FlipnoteEditorService = appState.FlipnoteEditorService;

            FramesManager = appState.FramesManager;
            FramesManager.CanvasModel = CanvasControl.CanvasModel;
            FramesManager.CurrentFrameChanged += FramesManager_CurrentFrameChanged;

            FramesViewer.FramesManager = FramesManager;

            FramesManager.EnsureCurrentFrameSelected();
            FramesManager.RequestThumbnailRedraw(FramesManager.GetCurrentFrame());

            GC.Collect();

            Task.Run(async () =>
            {
                var fm = appState.FramesManager;
                foreach (var f in fm.Frames)
                    await fm.RedrawThumbnailAsync(f);
            });
        }

        private void FramesManager_CurrentFrameChanged(object sender, EventArgs e)
        {
            ComponentsListView.SetData(FramesManager.GetCurrentFrame().Components);
        }
    }
}
