using FlipnoteDesktop.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Windows.FlipnoteUserIdGetterPages
{
    /// <summary>
    /// Interaction logic for ExtractFromFlipnotePage.xaml
    /// </summary>
    public partial class ExtractFromFlipnotePage : Page
    {
        public ExtractFromFlipnotePage()
        {
            InitializeComponent();
            FileSelector.FileExtension = "*.ppm";
            FileSelector.GetDrives();
        }        

        public FlipnoteUserIdGetterWindow Window;

        private void FileSelector_ConfirmRequest(object sender)
        {
            var path = System.IO.Path.Combine(FileSelector.Path, FileSelector.Filename);
            if(File.Exists(path))
            {
                var meta = Flipnote.ReadMetadata(path);
                Window.CheckIdentityPage.AuthorName = meta.CurrentAuthorName;
                Window.CheckIdentityPage.AuthorId = meta.CurrentAuthorId;
                Window.CheckIdentityPage.Source = this;
                Window.GoToCheckIdentityPage();
            }
            else
            {
                MessageBox.Show("An error occured.");
            }
        }

        private void FileSelector_BackRequest(object sender)
        {
            Window.GoToHomePage();
        }
    }
}
