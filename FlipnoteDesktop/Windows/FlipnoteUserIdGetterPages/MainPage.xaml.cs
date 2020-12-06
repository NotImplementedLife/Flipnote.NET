using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    internal partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (App.AuthorName != null)
            {
                IdRun.Text = string.Join("", App.AuthorId.Reverse().Select(t => t.ToString("X2")));
                UserRun.Text = App.AuthorName;
                CurrentUserData.Visibility = Visibility.Visible;
            }
            else
            {
                CurrentUserData.Visibility = Visibility.Collapsed;
            }
        }


        public FlipnoteUserIdGetterWindow Window;

        private void ExtractFromPPMHyperLink_Click(object sender, RoutedEventArgs e)                  
            => Window.GoToExtractFromPPMPage();                   

        private void ExtractFromUserDataHyperLink_Click(object sender, RoutedEventArgs e)        
            => Window.GoToExtractFromUserDataFilePage();        
    }
}
