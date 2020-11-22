using FlipnoteDesktop.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FlipnoteDesktop.Windows;
using FlipnoteDesktop.Data;

namespace FlipnoteDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Shortcut to the Application path
        /// </summary>
        public static string Path = AppDomain.CurrentDomain.BaseDirectory;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 1)
            {             
                var filename = e.Args[0];                
                var flipnote = new Flipnote(filename);
                double BorderWidth = (SystemParameters.MaximizedPrimaryScreenWidth - SystemParameters.FullPrimaryScreenWidth) / 2.0;
                double TitleBarHeight = BorderWidth + SystemParameters.WindowCaptionHeight;
                Window wnd = new Window
                {
                    Title = "Flipnote Player",
                    Width = 512,
                    Height = 384 + TitleBarHeight-2
                };
                SimpleFlipnotePlayer player = new SimpleFlipnotePlayer();                
                player.Source = flipnote;
                wnd.Content = player;
                wnd.Loaded += delegate(object o, RoutedEventArgs ev)
                {
                    player.Start();
                };
                wnd.Show();
            }
            else
            {
                new SplashWindow().Show();
            }

        }        
    }
}
