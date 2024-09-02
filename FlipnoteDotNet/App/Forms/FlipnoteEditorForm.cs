using FlipnoteDotNet.Utils.GUI;

namespace FlipnoteDotNet.App.Forms
{
    public partial class FlipnoteEditorForm : Form
    {
        private AppState AppState;
        public FlipnoteEditorForm(AppState appState)
        {
            InitializeComponent();
            AppState = appState;
            FlipnoteEditorContainer.Initialize(appState);
            UndoLinker.Connect(AppState.UndoStack, UndoButton, RedoButton);
            MainMenuStrip = menuStrip1;
        }

        private void NewFrameButton_Click(object sender, EventArgs e)
        {
            AppState.FlipnoteEditorService.InsertNewFrameAfterCurrent();
        }

        private void CopyCurrentFrameButton_Click(object sender, EventArgs e)
        {
            AppState.FlipnoteEditorService.DuplicateCurrentFrame();
        }

        private void RemoveCurrentFrameButton_Click(object sender, EventArgs e)
        {
            var service = AppState.FlipnoteEditorService;
            if (service.FramesCount == 1) 
            {
                if (AppState.FramesManager.Frames[0].Components.Count > 0)
                {
                    AppState.FlipnoteEditorService.ClearCurrentFrame();
                }
            }
            else
            {
                AppState.FlipnoteEditorService.RemoveCurrentFrame();
            }
        }
    }
}
