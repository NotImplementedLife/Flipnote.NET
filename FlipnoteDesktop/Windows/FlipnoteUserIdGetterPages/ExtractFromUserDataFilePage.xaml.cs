using FlipnoteDesktop.Data;
using FlipnoteDesktop.Extensions;
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
    /// Interaction logic for ExtractFromUserDataFilePage.xaml
    /// </summary>
    public partial class ExtractFromUserDataFilePage : Page
    {
        public ExtractFromUserDataFilePage()
        {
            InitializeComponent();
            FileSelector.FileExtension = "*.fsuserdata";
            FileSelector.GetDrives();
        }

        public FlipnoteUserIdGetterWindow Window;

        private void FileSelector_ConfirmRequest(object sender)
        {
            var path = System.IO.Path.Combine(FileSelector.Path, FileSelector.Filename);
            if (File.Exists(path))
            {
                try
                {
                    using (BinaryReader r = new BinaryReader(File.Open(path, FileMode.Open))) 
                    {
                        Window.CheckIdentityPage.AuthorName = r.ReadWChars(11);
                        Window.CheckIdentityPage.AuthorId = r.ReadBytes(8);                        
                    }
                    Window.CheckIdentityPage.Source = this;
                    Window.GoToCheckIdentityPage();
                }
                catch (Exception)
                {
                    Dispatcher.Invoke(() => MessageBox.Show("Could not load user data."));
                }               
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
