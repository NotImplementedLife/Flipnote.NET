using FlipnoteDesktop.Environment.Canvas;
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
            Image.Source = new WriteableBitmap(256, 192, 96, 96, PixelFormats.Indexed2, new BitmapPalette(new List<Color>
                { Colors.White, Colors.Black, Colors.Red, Colors.Blue}));
            DrawingTool = new PenTool();
            DrawingTool.Attach(this);
        }

        public DrawingTool DrawingTool;

        public static DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(int), typeof(FrameCanvasEditor),
            new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnZoomChanged)));

        public int Zoom
        {
            get => (int)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        internal void UpdateImage(bool forceBuild=false)
        {            
            if(forceBuild)
            {
                for(int x=0;x<256;x++)
                    for(int y=0;y<192;y++)
                    {
                        if (CanvasData1[x, y])
                            SetImagePixel(x, y, layer1Cl);
                        else if (CanvasData2[x, y])
                            SetImagePixel(x, y, layer2Cl);
                        else SetImagePixel(x, y, 0);
                    }
            }
            if (Image != null)
            {
                (Image.Source as WriteableBitmap).WritePixels(new Int32Rect(0, 0, 256, 192), pixels, 64, 0);
            }
        }

        
        byte[] pixels = new byte[64 * 192];
        private void SetImagePixel(int x,int y,int val)
        {
            int b = 256 * y + x;
            int p = 3 - b % 4;
            b /= 4;
            pixels[b] &= (byte)(~(0b11 << (2 * p)));
            pixels[b] |= (byte)(val << (2 * p));
        }

        internal void SetPixel(int x,int y)
        {
            if (!(0 <= x && x <= 255 && 0 <= y && y <= 191))
                return;
            if (LayerSelector.IsChecked!=true)
            {
                CanvasData1[x, y] = true;                
                    SetImagePixel(x, y, layer1Cl);
            }
            else
            {
                CanvasData2[x, y] = true;
                if(!CanvasData1[x,y])
                {
                    SetImagePixel(x, y, layer2Cl);
                }
            }
        }

        internal void ErasePixel(int x, int y)
        {
            if (LayerSelector.IsChecked != true)
            {
                CanvasData1[x, y] = false;
                SetImagePixel(x, y, CanvasData2[x, y] ? layer2Cl : 0);
            }
            else
            {
                CanvasData2[x, y] = false;
                if (!CanvasData1[x, y])
                {
                    SetImagePixel(x, y, 0);
                }
            }
        }

        private void SetMeasures(int zoom)
        {
            int prevZoom = (int)Container.Width / 256;
            var pos = Mouse.GetPosition(Body);
            var pX = Body.ActualWidth / 2 - pos.X;
            var pY = Body.ActualHeight / 2 - pos.Y;
            double scale = (double)zoom / prevZoom;

            DbgCanvasPos.Text = $"({pX}, {pY})";
            Body.Width = zoom * (256 + 50);
            Body.Height = zoom * (192 + 50);
            Container.Width = zoom * 256;
            Container.Height = zoom * 192;
            DrawingSurface.Width = zoom * 256;
            DrawingSurface.Height = zoom * 192;
            Grid.Width = zoom * 256;
            Grid.Height = zoom * 192;
            //ExtensionPanel.Width = zoom * 256;
            //ExtensionPanel.Height = zoom * 192;

            /// BUGGY !!!
            if (ScrollViewer.ScrollableWidth > 0) ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - pX * scale);
            if (ScrollViewer.ScrollableHeight > 0) ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - pY * scale);
        }

        private static void OnZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as FrameCanvasEditor;
            var zoom = (int)e.NewValue;
            editor.SetMeasures(zoom);

        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Add)
                ZoomIn();
            else if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.Subtract)
                ZoomOut();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ScrollableHeight / 2);
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.ScrollableWidth / 2);
        }

        bool[,] CanvasData1 = new bool[256, 192];
        bool[,] CanvasData2 = new bool[256, 192];


        internal int canvasX = 0, canvasY = 0;

        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;
            if (e.Delta > 0)
                ZoomIn();
            else if (e.Delta < 0)
                ZoomOut();
        }

        void ZoomIn()
        {
            Zoom++;
        }

        void ZoomOut()
        {
            if (Zoom > 1)
                Zoom--;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void DrawingSurface_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void DrawingSurface_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(DrawingSurface);
            canvasX = (int)(pos.X / Zoom);
            canvasY = (int)(pos.Y / Zoom);
            DbgCanvasPos.Text = $"({canvasX}, {canvasY})";           
        }        

        public void ToggleGridVisibility()
        {
            if (Grid.Visibility == Visibility.Visible)
                HideGrid();
            else
                ShowGrid();
        }

        public void ShowGrid()
        {
            Grid.Visibility = Visibility.Visible;
        }

        private void LayerSelector_Checked(object sender, RoutedEventArgs e)
        {

        }

        int layer1Cl = 1;
        int layer2Cl = 1;

        private void LayerBox1_ValueChanged(object o)
        {
            switch(LayerBox1.Value)
            {                
                case 0:
                    layer1Cl = 1;
                    UpdateImage(true);
                    break;
                case 1:
                    layer1Cl = 2;
                    UpdateImage(true);
                    break;
                case 2:
                    layer1Cl = 3;
                    UpdateImage(true);
                    break;
            }
        }

        private void LayerBox2_ValueChanged(object o)
        {
            switch (LayerBox2.Value)
            {
                case 0:
                    layer2Cl = 1;
                    UpdateImage(true);
                    break;
                case 1:
                    layer2Cl = 2;
                    UpdateImage(true);
                    break;
                case 2:
                    layer2Cl = 3;
                    UpdateImage(true);
                    break;
            }
        }

        public void HideGrid()
        {
            Grid.Visibility = Visibility.Collapsed;
        }
    }
}
