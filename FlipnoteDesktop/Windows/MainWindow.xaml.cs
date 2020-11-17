using FlipnoteDesktop.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            ShowGridMenuItem.IsChecked = FrameCanvasEditor.Grid.Visibility == Visibility.Visible;
        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ToggleGridVisibility_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FrameCanvasEditor.ToggleGridVisibility();
            ShowGridMenuItem.IsChecked = FrameCanvasEditor.Grid.Visibility == Visibility.Visible;
        }
    }

    static class MainWindowCommands
    {
        public static RoutedCommand ToggleGridVisibility = new RoutedCommand();
    }
}
