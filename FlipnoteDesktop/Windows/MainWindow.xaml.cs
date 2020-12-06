using FlipnoteDesktop.Data;
using FlipnoteDesktop.Environment.Canvas;
using FlipnoteDesktop.Environment.Canvas.Generators;
using FlipnoteDesktop.External.Plugins;
using Microsoft.Win32;
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
    internal partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
            // create a single blank frame
            FramesList.List.ItemsSource = new List<DecodedFrame>
            {
                new DecodedFrame()
            };
            App.AuthorNameChanged += App_AuthorNameChanged;
            FramesList.List.SelectedIndex = 0;
            if(App.AuthorName!=null)
            {
                FlipnoteUserMenuItem.InputGestureText = App.AuthorName;
            }

            ExampleGeneratorMenuItem.Tag = typeof(ExampleGenerator);
        }

        private void App_AuthorNameChanged()
        {            
            if (App.AuthorName != null)
            {
                FlipnoteUserMenuItem.InputGestureText = App.AuthorName;
            }
            else
            {
                FlipnoteUserMenuItem.InputGestureText = "(none)";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowGridMenuItem.IsChecked = FrameCanvasEditor.Grid.Visibility == Visibility.Visible;
            _ToggleBtnRef = RightTabControl.Template.FindName("TabControlToggle", RightTabControl) as ToggleButton;            
        }              

        #region RightTabControl

        /// <summary>
        /// A quick reference to the to the TabControlToggle.
        /// Value is set inside Window_Loaded
        /// </summary>
        ToggleButton _ToggleBtnRef = null;

        /// <summary>
        /// Event raised by TabControlToggle.
        /// When button checked (aka tab control expanded), the first tab gets focus.
        /// When button unchecked (aka tab control collapsed), all tabs loose focus.
        /// </summary>
        /// <param name="sender">Must be TabControlToggle</param>        
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

        /// <summary>
        /// Expands tab control when one of the tabs is selected
        /// </summary>       
        private void TabItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_ToggleBtnRef.IsChecked != true)
            {
                _ToggleBtnRef.IsChecked = true;
                TabControlToggle_Click(_ToggleBtnRef, null);
            }
        }

        #endregion

        #region Commands

        private void ToggleGridVisibility_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FrameCanvasEditor.ToggleGridVisibility();
            ShowGridMenuItem.IsChecked = FrameCanvasEditor.Grid.Visibility == Visibility.Visible;
        }

        private void ZoomInCanvas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FrameCanvasEditor.ZoomIn();
        }

        private void ZoomOutCanvas_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FrameCanvasEditor.ZoomOut();
        }

        private void FramesList_SingleFrameSelected(object o, DecodedFrame frame)
        {
            if(frame!=FrameCanvasEditor.Frame)
            {
                FrameCanvasEditor.Frame = frame;
            }
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Para Para Manga Koubou (*.ppm)|*.ppm",
                Multiselect = false,
                InitialDirectory=App.Path
            };
            if(ofd.ShowDialog()==true)
            {
                Flipnote = new Flipnote(ofd.FileName);
                // This stores the flipnote frames
                // Ok, ok, it's not the best practice, but at least it works.
                // Maybe I'll change this in the future.
                FramesList.List.ItemsSource = Flipnote.GetDecodedFrameList();                
                FramesList.List.Items.Refresh();
                FramesList.List.SelectedIndex = 0;
            }
        }

        private void SwitchActiveLayer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FrameCanvasEditor.LayerSelector.IsChecked = !(FrameCanvasEditor.LayerSelector.IsChecked == true);
        }

        private void GetFlipnoteUserId_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new FlipnoteUserIdGetterWindow().ShowDialog();            
        }

        private void InvertLayerColors_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var tmp = FrameCanvasEditor.LayerBox1.Value;
            FrameCanvasEditor.LayerBox1.Value = FrameCanvasEditor.LayerBox2.Value;
            FrameCanvasEditor.LayerBox2.Value = tmp;
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Flipnote == null)
            {
                Flipnote = Flipnote.New(App.AuthorName, App.AuthorId, FramesList.List.ItemsSource as List<DecodedFrame>);
                Flipnote.Save(Flipnote.Filename);
            }
            else
            {
                Flipnote = Flipnote.New(App.AuthorName, App.AuthorId, FramesList.List.ItemsSource as List<DecodedFrame>);
                Flipnote.Save(Flipnote.Filename);
            }
        }

        private void OpenPluginManager_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new PluginManagerWindow().ShowDialog();
        }
        #endregion

        Flipnote Flipnote = null;       
        
        public void GeneratorMenuItem_Click(object sender,RoutedEventArgs e)
        {
            (Activator.CreateInstance((sender as MenuItem).Tag as Type) as Generator)
                .Execute(FramesList.List.ItemsSource as List<DecodedFrame>);
            FramesList.List.Items.Refresh();
            FramesList.List.SelectedIndex = 0;
        }
    }   
}
