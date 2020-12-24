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

        static double BorderWidth = (SystemParameters.MaximizedPrimaryScreenWidth - SystemParameters.FullPrimaryScreenWidth) / 2.0;
        static double TitleBarHeight = BorderWidth + SystemParameters.WindowCaptionHeight;

        private void Application_Startup(object sender, StartupEventArgs e)
        {                            
            if (e.Args.Length == 1)
            {
                var filename = e.Args[0];
                var flipnote = new Flipnote(filename);

                CreateFlipnotePlayerWindow(flipnote).Show();
            }
            else if (e.Args.Length == 0)
            {
                new SplashWindow().Show();
            }
            else
            {

            }
        }

        public static Window CreateFlipnotePlayerWindow(Flipnote src)
        {
            Window wnd = new Window
            {
                Title = "Flipnote Player",
                Width = 512,
                Height = 384 + TitleBarHeight - 4
            };
            SimpleFlipnotePlayer player = new SimpleFlipnotePlayer();
            player.Source = src;
            wnd.Content = player;
            wnd.Loaded += delegate (object o, RoutedEventArgs ev)
            {
                player.Start();
            };
            return wnd;
        }
    }
}
