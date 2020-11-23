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
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        public FlipnoteUserIdGetterWindow Window;

        private void ExtractFromPPMHyperLink_Click(object sender, RoutedEventArgs e)                  
            => Window.GoToExtractFromPPMPage();                   

        private void ExtractFromUserDataHyperLink_Click(object sender, RoutedEventArgs e)        
            => Window.GoToExtractFromUserDataFilePage();        
    }
}
