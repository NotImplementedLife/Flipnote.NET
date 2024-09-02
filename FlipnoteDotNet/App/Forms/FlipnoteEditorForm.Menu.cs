using FlipnoteDotNet.Utils;

namespace FlipnoteDotNet.App.Forms
{
    public partial class FlipnoteEditorForm
    {        
        [Order]
        public void MenuⰭFileⰭNewⰭFlipnote_Project()
        {

        }

        [Order]
        public void MenuⰭFileⰭNewⰭFlipnote_3D_Project()
        {

        }

        [Order]
        public void MenuⰭFileⰭNewⰭVGA16_Project()
        {

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
