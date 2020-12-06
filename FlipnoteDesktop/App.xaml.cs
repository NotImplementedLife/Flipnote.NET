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
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Reflection;

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

        private static string _AuthorName = null;
        public static string AuthorName
        {
            get => _AuthorName;
            set
            {
                if (_AuthorName != value)
                {
                    _AuthorName = value;
                    AuthorNameChanged?.Invoke();
                }
            }
        }
        public static byte[] AuthorId = null;

        public delegate void OnAuthorNameChanged();
        public static event OnAuthorNameChanged AuthorNameChanged;

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
                    Height = 384 + TitleBarHeight - 4
                };
                SimpleFlipnotePlayer player = new SimpleFlipnotePlayer();
                player.Source = flipnote;
                wnd.Content = player;
                wnd.Loaded += delegate (object o, RoutedEventArgs ev)
                {
                    player.Start();
                };
                wnd.Show();
            }
            else if (e.Args.Length == 0)
            {
                new SplashWindow().Show();
            }
            else
            {

            }
        }                     
    }
}
