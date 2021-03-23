using FlipnoteDotNet.Windows.FlipnoteUserIdGetterPages;
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
using System.Windows.Shapes;

namespace FlipnoteDotNet.Windows
{
    /// <summary>
    /// Interaction logic for FlipnoteUserIdGetterWindow.xaml
    /// </summary>
    internal partial class FlipnoteUserIdGetterWindow : Window
    {
        public FlipnoteUserIdGetterWindow()
        {
            InitializeComponent();
            MainPage = new MainPage { Window = this };                        
            ExtractFromFlipnotePage = new ExtractFromFlipnotePage { Window = this };
            ExtractFromUserDataFilePage = new ExtractFromUserDataFilePage { Window = this };
            CheckIdentityPage = new CheckIdentityPage { Window = this };
            GoToHomePage();            
        }        

        public MainPage MainPage;
        public ExtractFromFlipnotePage ExtractFromFlipnotePage;
        public ExtractFromUserDataFilePage ExtractFromUserDataFilePage;
        public CheckIdentityPage CheckIdentityPage;

        public void GoToHomePage()        
            => Frame.Navigate(MainPage);

        public void GoToExtractFromUserDataFilePage()
            => Frame.Navigate(ExtractFromUserDataFilePage);       

        public void GoToExtractFromPPMPage()        
            => Frame.Navigate(ExtractFromFlipnotePage);       

        public void GoToCheckIdentityPage()        
            => Frame.Navigate(CheckIdentityPage);

        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
            => Frame.NavigationService.RemoveBackEntry();
        
    }
}
