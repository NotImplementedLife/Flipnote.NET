using FlipnoteDesktop.Data;
using FlipnoteDesktop.Environment.Canvas;
using FlipnoteDesktop.Environment.Canvas.Generators;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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
            PlayBackSpeed.Value = 3;
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

                AHFlags = Flipnote.AnimationHeader.Flags;
                UpdateProperties();
                PlayBackSpeed.Value = 8 - Flipnote.SoundHeader.CurrentFramespeed;
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
            if(App.AuthorName==null || App.AuthorId==null)
            {
                if (MessageBox.Show(
                    "Flipnote Studio user data is missing or was corrupted.\n" +
                    "Do you want to set up the user data?\n" +
                    "Clicking \"No\" will abort the saving attempt.", 
                    "Error", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (new FlipnoteUserIdGetterWindow().ShowDialog() != true)
                        return;
                }
                else return;
            }
            if (Flipnote == null)
            {
                Flipnote = Flipnote.New(App.AuthorName, App.AuthorId, FramesList.List.ItemsSource as List<DecodedFrame>);                
            }
            else
            {
                Flipnote = Flipnote.New(App.AuthorName, App.AuthorId, FramesList.List.ItemsSource as List<DecodedFrame>);                
                /// what the hell did I want to do here differently???
            }
            Flipnote.AnimationHeader.Flags = AHFlags;
            Flipnote.SoundHeader.CurrentFramespeed = (byte)(8 - PlayBackSpeed.Value);
            Flipnote.Save(Flipnote.Filename);
            MessageBox.Show("Flipnote saved!");
        }

        private void OpenPluginManager_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            new PluginManagerWindow().ShowDialog();
        }
        #endregion

        #region Events
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowGridMenuItem.IsChecked = FrameCanvasEditor.Grid.Visibility == Visibility.Visible;
            _ToggleBtnRef = RightTabControl.Template.FindName("TabControlToggle", RightTabControl) as ToggleButton;
        }

        private void FramesList_SingleFrameSelected(object o, DecodedFrame frame)
        {
            if (frame != FrameCanvasEditor.Frame)
            {
                FrameCanvasEditor.Frame = frame;
            }
        }

        public void GeneratorMenuItem_Click(object sender, RoutedEventArgs e)
        {
            (Activator.CreateInstance((sender as MenuItem).Tag as Type) as Generator)
                .Execute(FramesList.List.ItemsSource as List<DecodedFrame>);
            FramesList.List.Items.Refresh();
            FramesList.List.SelectedIndex = 0;
        }

        // to check out if AHFlags were changed from code side or user side
        bool CodeChanges = false;

        private void HideLayer1ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CodeChanges) return;
            AHFlags |= 0x0010;
        }

        private void HideLayer1ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CodeChanges) return;
            AHFlags &= 0xFFEF;
        }

        private void HideLayer2ChkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CodeChanges) return;
            AHFlags |= 0x0020;
        }

        private void HideLayer2ChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CodeChanges) return;
            AHFlags &= 0xFFDF;
        }

        private void LoopChkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CodeChanges) return;
            AHFlags |= 0x0001;
        }

        private void LoopChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CodeChanges) return;
            AHFlags &= 0xFFFE;
        }

        private void UpdateProperties()
        {
            LoopChkBox.IsChecked = (AHFlags & 0x0001) > 0;
            HideLayer1ChkBox.IsChecked = (AHFlags & 0x0010) > 0;
            HideLayer1ChkBox.IsChecked = (AHFlags & 0x0020) > 0;
        }        

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }
        #endregion


        Flipnote Flipnote = null;

        // a copy of Animation Header Flags
        uint AHFlags = 0x40;                   
    }   
}
