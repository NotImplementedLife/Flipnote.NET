using FlipnoteDesktop.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var f = new DecodedFrame();
            f.IsPaperWhite = true;
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 96; y++)
                    f.SetImagePixel(x, y, 2);
            f.CreateThumbnail();

            FramesList.List.ItemsSource = new List<DecodedFrame>
            {
                f
            };
            FrameCanvasEditor.Frame = f;
        }

        ToggleButton _ToggleBtnRef = null;
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowGridMenuItem.IsChecked = FrameCanvasEditor.Grid.Visibility == Visibility.Visible;
            _ToggleBtnRef = RightTabControl.Template.FindName("TabControlToggle", RightTabControl) as ToggleButton;            
        }

        private void ToggleGridVisibility_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FrameCanvasEditor.ToggleGridVisibility();
            ShowGridMenuItem.IsChecked = FrameCanvasEditor.Grid.Visibility == Visibility.Visible;
        }

        private void SwitchActiveLayer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FrameCanvasEditor.LayerSelector.IsChecked = !(FrameCanvasEditor.LayerSelector.IsChecked == true);            
        }        
    
        private void TabControlToggle_Click(object sender, RoutedEventArgs e)
        {            
            var btn = sender as ToggleButton;
            if(btn.IsChecked!=true)
            {
                RightTabControl.SelectedIndex = 0;
            }
            else
            {
                RightTabControl.SelectedIndex = 1;
            }
        }

        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_ToggleBtnRef.IsChecked != true)
            {
                _ToggleBtnRef.IsChecked = true;
                TabControlToggle_Click(_ToggleBtnRef, null);
            }
        }
    }

    static class MainWindowCommands
    {
        public static RoutedCommand ToggleGridVisibility = new RoutedCommand();
        public static RoutedCommand SwitchActiveLayer = new RoutedCommand();
    }
}
