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

namespace FlipnoteDesktop.Controls
{
    /// <summary>
    /// Interaction logic for FrameCanvasEditor.xaml
    /// </summary>
    public partial class FrameCanvasEditor : UserControl
    {
        public FrameCanvasEditor()
        {
            InitializeComponent();
            SetMeasures(Zoom);
        }

        public static DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(int), typeof(FrameCanvasEditor),
            new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnZoomChanged)));

        public int Zoom
        {
            get => (int)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }


        private void SetMeasures(int zoom)
        {
            double dy = 0;
            Body.Width = zoom * (256 + 50);
            Body.Height = zoom * (192 + 50);
            Container.Width = zoom * 256;
            Container.Height = zoom * 192;
            DrawingSurface.Width = zoom * 256;
            DrawingSurface.Height = zoom * 192;
            Grid.Width = zoom * 256;
            Grid.Height = zoom * 192;

            DrawingSurface.Children.Clear();
            for (int x = 0; x < 256; x++)
                for (int y = 0; y < 192; y++) 
                    if(CanvasData[x,y])
                        SetCanvasPixel(x, y);

        }

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as FrameCanvasEditor;
            var zoom = (int)e.NewValue;
            editor.SetMeasures(zoom);

        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Add) Zoom++;
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Subtract && Zoom > 1) Zoom--;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ScrollableHeight / 2);
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.ScrollableWidth / 2);
        }

        bool[,] CanvasData = new bool[256, 192];

        private void DrawingSurface_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetCanvasPixel(canvasX, canvasY);
        }

        int canvasX = 0, canvasY = 0;

        private void DrawingSurface_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(DrawingSurface);
            canvasX = (int)(pos.X / Zoom);
            canvasY = (int)(pos.Y / Zoom);
            DbgCanvasPos.Text = $"({canvasX}, {canvasY})";
            if(Mouse.LeftButton == MouseButtonState.Pressed)
            {
                SetCanvasPixel(canvasX,canvasY);
            }
        }

        void SetCanvasPixel(int x,int y)
        {
            CanvasData[x, y] = true;
            var sq = new Rectangle();
            sq.Fill = new SolidColorBrush(Colors.Black);
            sq.Width = Zoom;
            sq.Height = Zoom;
            Canvas.SetLeft(sq, Zoom * x);
            Canvas.SetTop(sq, Zoom * y);
            DrawingSurface.Children.Add(sq);
        }
    }
}
