using FlipnoteDotNet.App.Storage;
using FlipnoteDotNet.Utils;
using System.Diagnostics;

namespace FlipnoteDotNet.App.Forms
{
    public partial class FlipnoteEditorForm
    {        
        [Order]
        public void MenuⰭFileⰭNewⰭFlipnote_Project()
        {
            ChangeState(AppState.CreateFlipnoteInstance(AppState.UndoStack));
        }

        [Order]
        public void MenuⰭFileⰭNewⰭFlipnote_3D_Project()
        {
            ChangeState(AppState.CreateFlipnote3DInstance(AppState.UndoStack));
        }

        [Order]
        public void MenuⰭFileⰭNewⰭVGA16_Project()
        {
            ChangeState(AppState.CreateVGA16Instance(AppState.UndoStack));
        }

        [Order]
        public void MenuⰭFileⰭOpen()
        {
            BytesContainer.Clear();
            var ser = new Serializer();
            var f = File.OpenRead("proj.zip");
            var project = ser.Deserialize<Project>(f);
            f.Close();
            var appState = Storage.Codecs.V1.DecodeProject(project, AppState);
            ChangeState(appState);            
        }


        [Order]
        public void MenuⰭFileⰭSave()
        {
            BytesContainer.Clear();
            var project = Storage.Codecs.V1.EncodeProject(AppState);
            var ser = new Serializer();
            var f = File.Create("proj.zip");
            ser.Serialize(f, project);
            f.Close();
            Environment.Exit(0);            
        }

        [Order]
        public void MenuⰭFileⰭExit()
        {
            
        }


        [Order]
        public void MenuⰭHelpⰭAbout()
        {
            MessageBox.Show($"Flipnote.NET {new Version(ProductVersion)}\nby NotImplementedLife\n2024", "About");
        }
    }
}
