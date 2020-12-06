using FlipnoteDesktop.Extensions;
using FlipnoteDesktop.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Windows
{
    /// <summary>
    /// Interaction logic for SplashWindow.xaml
    /// </summary>
    internal partial class SplashWindow : Window
    {
        public SplashWindow()
        {
            InitializeComponent();
            BuildVersion.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();
            Loaded += SplashWindow_Loaded;
        }

        /// <summary>
        /// Show logo for 3 seconds, then launch the MainWindow
        /// </summary>      
        private void SplashWindow_Loaded(object sender, RoutedEventArgs e)
        {           
            Task.Run(() =>
            {
                // check for .fsuserdata
                if (File.Exists(".fsuserdata")) 
                {
                    try
                    {                               
                        using (BinaryReader r = new BinaryReader(File.Open(".fsuserdata", FileMode.Open)))
                        {

                            Dispatcher.Invoke(() => App.AuthorName = r.ReadWChars(11).Trim('\0'));                            
                            Dispatcher.Invoke(() => App.AuthorId = r.ReadBytes(8));
                            
                        }                        
                    }
                    catch (Exception)
                    {
                        Dispatcher.Invoke(()=>MessageBox.Show("Could not load user data."));
                    }
                }
                
                Thread.Sleep(3000);
                Dispatcher.Invoke(() =>
                {                    
                    new MainWindow().Show();
                    Close();                    
                });
            });
        }       
    }
}
