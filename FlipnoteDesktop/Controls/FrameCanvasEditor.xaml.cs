using FlipnoteDesktop.Data;
using FlipnoteDesktop.Environment.Canvas;
using FlipnoteDesktop.Environment.Canvas.DrawingTools;
using FlipnoteDesktop.Windows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            Image.Source = new WriteableBitmap(256, 192, 96, 96, PixelFormats.Indexed2, DecodedFrame.Palette);
            DrawingTool = new BrushTool();
            DrawingTool.Attach(this);
            ToolBox.Target = this;
        }

        public DrawingTool DrawingTool;

        public static DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(int), typeof(FrameCanvasEditor),
            new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnZoomPropertyChanged)));

        public int Zoom
        {
            get => (int)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        private DecodedFrame _Frame = new DecodedFrame() { IsPaperWhite = true };
        public DecodedFrame Frame
        {
            get => _Frame;
            set
            {
                if (_Frame != value)
                {
                    _Frame = value;
                    LayerBox1.Value = Frame.Layer1Color;
                    LayerBox2.Value = Frame.Layer2Color;
                    PaperColorSelector.IsChecked = !Frame.IsPaperWhite;                    
                    UpdateImage(true);
                    FrameChanged?.Invoke(this);
                }
            }
        }

        public delegate void OnFrameChanged(object o);
        public event OnFrameChanged FrameChanged;

        internal void UpdateImage(bool forceRedraw = false)
        {
            if (Image != null)
            {
                Frame.SetImage(Image.Source as WriteableBitmap, forceRedraw);
                //(Image.Source as WriteableBitmap).WritePixels(new Int32Rect(0, 0, 256, 192), pixels, 64, 0);
            }
        }

        public int SelectedLayer { get => LayerSelector.IsChecked == true ? 2 : 1; }

        public void SetPixel(int x, int y) => Frame.SetPixel(SelectedLayer, x, y);
        public void ErasePixel(int x, int y) => Frame.ErasePixel(SelectedLayer, x, y);
        public void SetPixels(bool[,] pixels) => Frame.SetLayerPixels(SelectedLayer, pixels);

        byte[] pixels = new byte[64 * 192];

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

        private static void OnZoomPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as FrameCanvasEditor;
            var zoom = (int)e.NewValue;
            editor.SetMeasures(zoom);
            editor.ZoomChanged?.Invoke(editor);
        }

        public delegate void OnZoomChanged(object o);
        public event OnZoomChanged ZoomChanged;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ScrollableHeight / 2);
            ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.ScrollableWidth / 2);
        }        

        #region Zoom Logic
        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;
            if (e.Delta > 0)
                ZoomIn();
            else if (e.Delta < 0)
                ZoomOut();
        }

        public void ZoomIn()
        {
            Zoom++;
        }

        public void ZoomOut()
        {
            if (Zoom > 1)
                Zoom--;
        }
        #endregion

        public bool[,] CurrentLayerPixelData
        {
            get => Frame.GetLayerPixels(SelectedLayer);
        }

        internal int canvasX = 0, canvasY = 0;

        private void DrawingSurface_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(ToolBox.isDragging)
            {
                e.Handled = true;
                return;
            }
            var pos = e.GetPosition(DrawingSurface);
            canvasX = (int)(pos.X / Zoom);
            canvasY = (int)(pos.Y / Zoom);
            DbgCanvasPos.Text = $"({canvasX}, {canvasY})";
        }

        #region Show/Hide Grid Logic
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

        public void HideGrid()
        {
            Grid.Visibility = Visibility.Collapsed;
        }
        #endregion

        private void LayerBox1_ValueChanged(object o)
        {
            Frame.Layer1Color = LayerBox1.Value;
            UpdateImage(true);
        }

        private void LayerBox2_ValueChanged(object o)
        {
            Frame.Layer2Color = LayerBox2.Value;
            UpdateImage(true);
        }


        private void PaperColorSelector_Checked(object sender, RoutedEventArgs e)
        {
            Frame.IsPaperWhite = false;
            UpdateImage(true);
        }

        private void PaperColorSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            Frame.IsPaperWhite = true;
            UpdateImage(true);
        }        

        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!ToolBox.isDragging) return;            
            var pt = e.GetPosition(ToolBox);
            Canvas.SetLeft(ToolBox,pt.X + Canvas.GetLeft(ToolBox) - ToolBox.dX);
            Canvas.SetTop(ToolBox, pt.Y + Canvas.GetTop(ToolBox) - ToolBox.dY);            
        }

        private void LayerSelector_Checked(object sender, RoutedEventArgs e)
        {
            SelectedLayerChanged?.Invoke(this);
        }

        private void LayerSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            SelectedLayerChanged?.Invoke(this);
        }

        public delegate void OnSelectedLayerChanged(object o);
        public event OnSelectedLayerChanged SelectedLayerChanged;

        private void PlayFlipnoteButton_Click(object sender, RoutedEventArgs e)
        {
            var wnd = Window.GetWindow(this) as MainWindow;
            var flipnote = Flipnote.New(null, null, wnd.FramesList.List.ItemsSource as List<DecodedFrame>, true);
            App.CreateFlipnotePlayerWindow(flipnote).ShowDialog();
        }

        public void ForceToolBoxDrag(Point pt)
        {            
            Canvas.SetLeft(ToolBox, pt.X + Canvas.GetLeft(ToolBox) - ToolBox.dX);
            Canvas.SetTop(ToolBox, pt.Y + Canvas.GetTop(ToolBox) - ToolBox.dY);
        }       
    }
}
