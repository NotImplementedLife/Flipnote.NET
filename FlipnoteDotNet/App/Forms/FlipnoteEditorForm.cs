using FlipnoteDotNet.App.Storage;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.GUI;
using System.Diagnostics;

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

        private void AdjustFormTitle(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                Text = $"Flipnote.NET";
            else
                Text = $"Flipnote.NET - {Path.GetFileNameWithoutExtension(filename)}";
        }

        private void ChangeState(AppState state)
        {
            if (!AppState.Changed) 
            {
                FlipnoteEditorContainer.ChangeState(state);
                AppState = state;
                AdjustFormTitle(AppState.Filename);
                return;
            }            
            switch(Prompts.SaveBeforeLoadNew())
            {
                case PromptResult.Save:
                    // save logic..
                    goto case PromptResult.Discard;

                case PromptResult.Discard:
                    // do
                    FlipnoteEditorContainer.ChangeState(state);
                    AppState = state;
                    AdjustFormTitle(AppState.Filename);
                    //FlipnoteEditorContainer
                    break;

                default: break;
            }            
        }

        private void LoadProject(string path)
        {
            BytesContainer.Clear();
            var ser = new Serializer();
            var f = File.OpenRead(path);
            var project = ser.Deserialize<Project>(f);
            f.Close();
            var appState = Storage.Codecs.V1.DecodeProject(project, AppState, FlipnoteEditorContainer.CanvasModel);
            appState.Filename = path;
            ChangeState(appState);
        }

        private void SaveProject(string path)
        {
            BytesContainer.Clear();
            var project = Storage.Codecs.V1.EncodeProject(AppState);
            var ser = new Serializer();
            var f = File.Create(path);
            ser.Serialize(f, project);
            f.Close();
            AppState.Filename = path;
        }

        
    }
}
