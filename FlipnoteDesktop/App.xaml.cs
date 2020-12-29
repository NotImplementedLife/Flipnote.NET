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
using FlipnoteDesktop.Environment.CommandLine;

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

        public static string Version = Assembly.GetEntryAssembly().GetName().Version.ToString();

        private void Application_Startup(object sender, StartupEventArgs e)
        {       
            if(CmdParser.FileName==null)
            {
                new SplashWindow().Show();
            }
            if(CmdParser.FileName!=null)
            {
                if(!CmdParser.OpenEdit)
                {
                    var filename = e.Args[0];
                    var flipnote = new Flipnote(filename);

                    CreateFlipnotePlayerWindow(flipnote).Show();
                }
                else
                {
                    new SplashWindow().Show();
                }
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
