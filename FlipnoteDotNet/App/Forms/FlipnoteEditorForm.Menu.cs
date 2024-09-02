using FlipnoteDotNet.Utils;

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
