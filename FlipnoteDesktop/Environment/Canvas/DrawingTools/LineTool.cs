using FlipnoteDesktop.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Environment.Canvas.DrawingTools
{
    public class LineTool : DrawingTool
    {
        public LineTool()
        {
            Attached += LineTool_Attached;
            Detached += LineTool_Detached;
        }

        Path PreviewLine = new Path()
        {
            IsHitTestVisible = false,
            Stroke = Brushes.Green,
            Visibility = System.Windows.Visibility.Collapsed
        };
        Ellipse PreviewLineCapStart = new Ellipse()
        {
            IsHitTestVisible = false,
            Stroke = Brushes.Green,
            Fill = Brushes.White,
            Width = 3,
            Height = 3,
            Visibility = System.Windows.Visibility.Collapsed
        };
        Ellipse PreviewLineCapEnd = new Ellipse()
        {
            IsHitTestVisible = false,
            Stroke = Brushes.Green,
            Fill = Brushes.White,
            Width = 3,
            Height = 3,            
            Visibility = System.Windows.Visibility.Collapsed
        };


        NumericInput SizeInput;
        PatternSelector PatternInput;

        public void LineTool_Attached(object o)
        {
            SizeInput = new NumericInput
            {
                MaxValue = 20,
                MinValue = 1,
                Width = 40,
                Height = 21
            };
            SizeInput.Value = Size;
            SizeInput.ValueChanged += SizeInput_ValueChanged;

            PatternInput = new PatternSelector
            {
                Height = 30,
                Width = 50,
            };
            PatternInput.ValueChanged += PatternInput_ValueChanged;
            
            Target.ToolOptions.Children.Add(new TextBlock
            {
                Text = "Line width : ",
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = 21,
                Margin = new System.Windows.Thickness(3, 0, 3, 0)
            });
            Target.ToolOptions.Children.Add(SizeInput);
            Target.ToolOptions.Children.Add(new TextBlock
            {
                Text = "Pattern : ",
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = 21,
                Margin = new System.Windows.Thickness(3, 0, 3, 0)
            });
            Target.ToolOptions.Children.Add(PatternInput);
            Target.ExtensionPanel.Children.Add(PreviewLine);
            Target.ExtensionPanel.Children.Add(PreviewLineCapStart);
            Target.ExtensionPanel.Children.Add(PreviewLineCapEnd);            
            Target.ExtensionPanel.ClipToBounds = true;
        }

        private void PatternInput_ValueChanged(object o)
        {
            Pattern = PatternInput.Value;
        }

        private void SizeInput_ValueChanged(object o)
        {
            Size = SizeInput.Value;
        }

        public void LineTool_Detached(object o)
        {
            SizeInput = null;
        }

        public int Size = 1;
        public Pattern Pattern = Patterns.Mono;

        void PutPoint(int x, int y)
        {
            int maxX = x + Size / 2 - 1 + (Size & 1);
            int maxY = y + Size / 2 - 1 + (Size & 1);
            for (int _x = Math.Max(0, x - Size / 2); _x <= maxX; _x++)
            {
                for (int _y = Math.Max(0, y - Size / 2); _y <= maxY; _y++)
                    if (Pattern.GetPixelAt(_x, _y))
                        Target.SetPixel(_x, _y);
            }
            Target.UpdateImage();
        }

        void ErasePoint(int x, int y)
        {
            int maxX = x + Size / 2 - 1 + (Size & 1);
            int maxY = y + Size / 2 - 1 + (Size & 1);
            for (int _x = Math.Max(0, x - Size / 2); _x <= maxX; _x++)
            {
                for (int _y = Math.Max(0, y - Size / 2); _y <= maxY; _y++)
                    Target.ErasePixel(_x, _y);
            }
            Target.UpdateImage();
        }

        bool IsLineSet = false;
        int X1, Y1, X2, Y2;

        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsLineSet)
                {
                    X1 = Target.canvasX;
                    Y1 = Target.canvasY;
                    System.Windows.Controls.Canvas.SetLeft(PreviewLineCapStart, Target.Zoom * (X1 + .5) - 3);
                    System.Windows.Controls.Canvas.SetTop(PreviewLineCapStart, Target.Zoom * (Y1 + .5) - 3);
                    PreviewLineCapStart.Visibility = System.Windows.Visibility.Visible;
                    PreviewLineCapEnd.Visibility = System.Windows.Visibility.Collapsed;
                    PreviewLine.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }
        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            PreviewLineCapStart.Width = PreviewLineCapStart.Height = 6;
            PreviewLineCapEnd.Width = PreviewLineCapEnd.Height = 6;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                X2 = Target.canvasX;
                Y2 = Target.canvasY;
                System.Windows.Controls.Canvas.SetLeft(PreviewLineCapEnd, Target.Zoom * (X2 + .5) - 3);
                System.Windows.Controls.Canvas.SetTop(PreviewLineCapEnd, Target.Zoom * (Y2 + .5) - 3);
                PreviewLine.Data = Geometry.Parse($"M{Target.Zoom * (X1 + .5f)} {Target.Zoom * (Y1 + .5f)}L{Target.Zoom * (X2 + .5f)} {Target.Zoom * (Y2 + .5f)}");
                PreviewLineCapEnd.Visibility = System.Windows.Visibility.Visible;
                PreviewLine.Visibility = System.Windows.Visibility.Visible;
            }
        }

        protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
        }

        protected override void OnZoomChanged(object sender)
        {
            float cX1 = Target.Zoom * (X1 + .5f);
            float cY1 = Target.Zoom * (Y1 + .5f);
            float cX2 = Target.Zoom * (X2 + .5f);
            float cY2 = Target.Zoom * (Y2 + .5f);

            System.Windows.Controls.Canvas.SetLeft(PreviewLineCapStart, cX1 - 3);
            System.Windows.Controls.Canvas.SetTop(PreviewLineCapStart, cY1 - 3);
            System.Windows.Controls.Canvas.SetLeft(PreviewLineCapEnd, cX2 - 3);
            System.Windows.Controls.Canvas.SetTop(PreviewLineCapEnd, cY2 - 3);
            PreviewLine.Data = Geometry.Parse($"M{cX1} {cY1}L{cX2} {cY2}");
        }
    }
}
