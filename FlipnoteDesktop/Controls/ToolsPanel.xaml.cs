using FlipnoteDesktop.Environment.Canvas;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ToolsPanel.xaml
    /// </summary>
    public partial class ToolsPanel : UserControl
    {
        public ToolsPanel()
        {
            InitializeComponent();
        }

        public bool isDragging = false;
        public int dX, dY;

        private void DragArea_MouseDown(object sender, MouseButtonEventArgs e)
        {            
            var pt = e.GetPosition(this);
            dX = (int)pt.X;
            dY = (int)pt.Y;
        }

        public FrameCanvasEditor Target;

        private void DragArea_MouseMove(object sender, MouseEventArgs e)
        {
            isDragging = (Mouse.LeftButton == MouseButtonState.Pressed);
            if (isDragging)
            {
                Target.ForceToolBoxDrag(e.GetPosition(this));
            }           
        }

        public DrawingTool DrawingTool;
        private Button SelectedToolButton = null;

        private static readonly Brush DefaultBtnBackground = new SolidColorBrush(Colors.Transparent);
        private static readonly Brush DefaultBtnForeground = new SolidColorBrush(Color.FromRgb(249, 115, 0));
        private static readonly Brush SelectedBtnBackground = new SolidColorBrush(Color.FromRgb(249, 115, 0));
        private static readonly Brush SelectedBtnForeground = new SolidColorBrush(Colors.White);


        private void ToolIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            var tag = btn.Tag;           
            if (tag != null)
            {                
                if (Target.DrawingTool != null)
                {
                    Target.DrawingTool.Detach();
                }                                
                Target.DrawingTool = Activator.CreateInstance(Type.GetType($"FlipnoteDesktop.Environment.Canvas.DrawingTools.{tag}")) as DrawingTool;
                Target.DrawingTool.Attach(Target);
                if (SelectedToolButton != null)
                {
                    SelectedToolButton.Background = DefaultBtnBackground;
                    SelectedToolButton.Foreground = DefaultBtnForeground;
                }
                SelectedToolButton = btn;
                SelectedToolButton.Background = SelectedBtnBackground;
                SelectedToolButton.Foreground = SelectedBtnForeground;
            }            
        }

        private void DragArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }
    }            
}
