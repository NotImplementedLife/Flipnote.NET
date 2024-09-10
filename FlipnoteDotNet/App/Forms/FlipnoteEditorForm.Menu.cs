using FlipnoteDotNet.App.Storage;
using FlipnoteDotNet.Utils;
using FlipnoteDotNet.Utils.Constants;
using FlipnoteDotNet.Utils.GUI;
using System.Diagnostics;

namespace FlipnoteDotNet.App.Forms
{
    public partial class FlipnoteEditorForm
    {        
        [MenuAction]
        public void MenuⰭFileⰭNewⰭFlipnote_Project()
        {
            ChangeState(AppState.CreateFlipnoteInstance(AppState.UndoStack));
        }

        [MenuAction]
        public void MenuⰭFileⰭNewⰭFlipnote_3D_Project()
        {
            ChangeState(AppState.CreateFlipnote3DInstance(AppState.UndoStack));
        }

        [MenuAction]
        public void MenuⰭFileⰭNewⰭVGA16_Project()
        {
            ChangeState(AppState.CreateVGA16Instance(AppState.UndoStack));
        }        

        [MenuAction]
        public void MenuⰭFileⰭOpen()
        {
            if (Dialogs.RequestOpenFile(out var filename, filter: Strings.FnProjFilter))
            {
                LoadProject(filename);
            }                       
        }


        [MenuAction]
        public void MenuⰭFileⰭSave()
        {
            var filename = AppState.Filename;
            if (string.IsNullOrEmpty(filename))
                Dialogs.RequestSaveFile(out filename, filter: Strings.FnProjFilter);
            if (!string.IsNullOrEmpty(filename))
                SaveProject(filename);
        }

        [MenuAction]
        public void MenuⰭFileⰭSave_As()
        {
            if(Dialogs.RequestSaveFile(out var filename, filter: Strings.FnProjFilter))
            {
                SaveProject(filename);
            }
        }

        [MenuAction]
        public void MenuⰭFileⰭExit()
        {
            
        }


        [MenuAction]
        public void MenuⰭHelpⰭAbout()
        {
            MessageBox.Show($"Flipnote.NET {new Version(ProductVersion)}\nby NotImplementedLife\n2024", "About");
        }
    }
}
