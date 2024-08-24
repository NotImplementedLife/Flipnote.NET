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
            FramesManager.AddNewFrame();
            FramesManager.SetCurrentFrame(0);

            InitializeLayout();
            InitializeAssets(appState.AssetsService);
            InitializeFrames(appState.FlipnoteEditorService);

            FramesViewer.FramesManager = FramesManager;
            FramesManager.RequestThumbnailRedraw(FramesManager.GetCurrentFrame());

            var button = new Button { Text = "Change Palette" };
            button.Click += (o, e) => { FramesManager.GetCurrentFrame().FrameConfig.ColorIndices = new int[] { 3, 0, 1 }; };
            Panel2.Controls.Add(button);            
        }        

        private static Action<object, PaintEventArgs> Painter(Color color)
            => (sender, e) => e.Graphics.Clear(color);                            
    }
}
