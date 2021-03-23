using FlipnoteDotNet.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlipnoteDotNet.Controls
{
    /// <summary>
    /// Interaction logic for SimpleFlipnotePlayer.xaml
    /// </summary>
    public partial class SimpleFlipnotePlayer : UserControl
    {
        public SimpleFlipnotePlayer()
        {
            InitializeComponent();                   
        }

        private Flipnote _Source;
        public Flipnote Source
        {
            get => _Source;
            set
            {
                _Source = value;                
                RenderSurface.Source = _Source.RenderFrame(0);                
            }
        }

        private bool TaskWorking = false;
        
        public void Start()
        {
            TaskWorking = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                int k = 0;
                Debug.WriteLine(Source.SoundHeader.CurrentFramespeed);
                while (true)
                {
                    if (TaskWorking)
                    {
                        Dispatcher.Invoke(() => RenderSurface.Source = _Source.RenderFrame(k++));
                    }
                    if (k == Source.Frames.Length) k = 0;                    
                    Thread.Sleep(Source.FrameSpeed);
                }
            });
        }
    }
}
