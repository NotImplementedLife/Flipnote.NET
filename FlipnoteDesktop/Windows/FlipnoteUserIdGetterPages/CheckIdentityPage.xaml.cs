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
    /// Interaction logic for CheckIdentityPage.xaml
    /// </summary>
    public partial class CheckIdentityPage : Page
    {
        public CheckIdentityPage()
        {
            InitializeComponent();
        }

        public Page Source = null;

        private string _AuthorName;
        public string AuthorName
        {
            get => _AuthorName;
            set
            {
                _AuthorName = value;
                Username.Text = value.Trim('\0');
            }
        }
        public byte[] AuthorId;
        public FlipnoteUserIdGetterWindow Window;

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatingMessage.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                var bytes = new byte[30];
                Array.Copy(Encoding.Unicode.GetBytes(AuthorName), bytes, 22);
                Array.Copy(AuthorId, 0, bytes, 22, 8);
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(App.Path, ".fsuserdata"), bytes); 
                Dispatcher.Invoke(() =>
                {
                    App.AuthorName = AuthorName.Trim('\0');
                    App.AuthorId = AuthorId;
                    Window.Close();
                });
            });
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            if(Source!=null)
            {
                Window.Frame.Navigate(Source);
            }
        }
    }
}
