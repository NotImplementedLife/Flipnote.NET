using FlipnoteDesktop.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Environment.Canvas.DrawingTools
{
    internal class LineTool : DrawingTool
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
            Width = 10,
            Height = 10,
            Visibility = Visibility.Collapsed
        };
        Ellipse PreviewLineCapEnd = new Ellipse()
        {
            IsHitTestVisible = false,
            Stroke = Brushes.Green,
            Fill = Brushes.White,
            Width = 10,
            Height = 10,            
            Visibility = Visibility.Collapsed
        };


        NumericInput SizeInput;
        BrushPatternSelector PatternInput;

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

            PatternInput = new BrushPatternSelector
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
            if (IsLineSet)
            {
                RestorePixels();
                DrawLine();
            }
        }

        private void SizeInput_ValueChanged(object o)
        {
            Size = SizeInput.Value;
            if(IsLineSet)
            {
                RestorePixels();
                DrawLine();
            }
        }

        public void LineTool_Detached(object o)
        {
            SizeInput = null;
        }

        public int Size = 1;
        public Pattern Pattern = BrushPatterns.Mono;

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

        bool IsLineSet = false;
        int X1, Y1, X2, Y2;

        bool[,] pixelsBackup = new bool[256, 192];
        int SelectedCap = 0;
        bool MouseMoved = false;
        bool MouseDown = false;

        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            int x = Target.canvasX;
            int y = Target.canvasY;
            MouseMoved = false;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                MouseDown = true;
                int d = 10 / Target.Zoom + 1;
                if(IsLineSet)
                {
                    if (Math.Abs(x - X1) <= d && Math.Abs(y - Y1) <= d)
                    {
                        SelectedCap = 1;
                    }
                    else if (Math.Abs(x - X2) <= d && Math.Abs(y - Y2) <= d) 
                    {
                        SelectedCap = 2;
                    }
                    else IsLineSet = false;
                }
                if (!IsLineSet)
                {
                    Array.Copy(Target.CurrentLayerPixelData, pixelsBackup, 256 * 192);
                    X1 = x;
                    Y1 = y;
                    System.Windows.Controls.Canvas.SetLeft(PreviewLineCapStart, Target.Zoom * (x + .5) - 5);
                    System.Windows.Controls.Canvas.SetTop(PreviewLineCapStart, Target.Zoom * (y + .5) - 5);
                    PreviewLineCapStart.Visibility = Visibility.Visible;
                    PreviewLineCapEnd.Visibility = Visibility.Collapsed;
                    PreviewLine.Visibility = Visibility.Collapsed;                    
                }                
            }
        }

        void PutPoint(int x, int y, bool updateImage = true)
        {
            int maxX = x + Size / 2 - 1 + (Size & 1);
            int maxY = y + Size / 2 - 1 + (Size & 1);
            for (int _x = Math.Max(0, x - Size / 2); _x <= maxX; _x++)
            {
                for (int _y = Math.Max(0, y - Size / 2); _y <= maxY; _y++)
                    if (Pattern.GetPixelAt(_x, _y))
                        Target.SetPixel(_x, _y);
            }
            if (updateImage)
                Target.UpdateImage();
        }

        void DrawLine()
        {
            var pts = LineTool.GetLinePixels(X1, Y1, X2, Y2);
            for (int i = 0; i < pts.Count; i++)
                PutPoint(pts[i].X, pts[i].Y, false);
            Target.UpdateImage();
        }

        void RestorePixels()
        {
            Target.SetPixels(pixelsBackup);           
            //Array.Copy(pixelsBackup, Target.SelectedLayer == 1 ? Target.Frame.Layer1Data : Target.Frame.Layer2Data, 256 * 192);
        }

        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDown && Mouse.LeftButton == MouseButtonState.Pressed) 
            {
                MouseMoved = true;
                if (!IsLineSet)
                {
                    X2 = Target.canvasX;
                    Y2 = Target.canvasY;
                    System.Windows.Controls.Canvas.SetLeft(PreviewLineCapEnd, Target.Zoom * (X2 + .5) - 5);
                    System.Windows.Controls.Canvas.SetTop(PreviewLineCapEnd, Target.Zoom * (Y2 + .5) - 5);
                    PreviewLine.Data = Geometry.Parse($"M{Target.Zoom * (X1 + .5f)} {Target.Zoom * (Y1 + .5f)}L{Target.Zoom * (X2 + .5f)} {Target.Zoom * (Y2 + .5f)}");
                    RestorePixels();
                    DrawLine();
                    PreviewLineCapEnd.Visibility = Visibility.Visible;
                    PreviewLine.Visibility = Visibility.Visible;
                }
                else if (SelectedCap != 0) 
                {                    
                    if(SelectedCap==1)
                    {
                        X1 = Target.canvasX;
                        Y1 = Target.canvasY;
                        System.Windows.Controls.Canvas.SetLeft(PreviewLineCapStart, Target.Zoom * (X1 + .5) - 5);
                        System.Windows.Controls.Canvas.SetTop(PreviewLineCapStart, Target.Zoom * (Y1 + .5) - 5);
                    }
                    else
                    {
                        X2 = Target.canvasX;
                        Y2 = Target.canvasY;
                        System.Windows.Controls.Canvas.SetLeft(PreviewLineCapEnd, Target.Zoom * (X2 + .5) - 5);
                        System.Windows.Controls.Canvas.SetTop(PreviewLineCapEnd, Target.Zoom * (Y2 + .5) - 5);
                    }
                    PreviewLine.Data = Geometry.Parse($"M{Target.Zoom * (X1 + .5f)} {Target.Zoom * (Y1 + .5f)}L{Target.Zoom * (X2 + .5f)} {Target.Zoom * (Y2 + .5f)}");
                    RestorePixels();
                    DrawLine();
                }
            }
        }

        protected override void OnMouseUp(object sender, MouseEventArgs e)
        {            
            if(!IsLineSet)
            {
                if (!MouseMoved)
                {
                    PreviewLineCapStart.Visibility = Visibility.Collapsed;
                    PreviewLineCapEnd.Visibility = Visibility.Collapsed;
                    PreviewLine.Visibility = Visibility.Collapsed;
                }
                else
                {
                    IsLineSet = true;
                }
            }
            else
            {
                SelectedCap = 0;
            }
            MouseDown = false;
            MouseMoved = false;
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

            System.Windows.Controls.Canvas.SetLeft(PreviewLineCapStart, cX1 - 5);
            System.Windows.Controls.Canvas.SetTop(PreviewLineCapStart, cY1 - 5);
            System.Windows.Controls.Canvas.SetLeft(PreviewLineCapEnd, cX2 - 5);
            System.Windows.Controls.Canvas.SetTop(PreviewLineCapEnd, cY2 - 5);
            PreviewLine.Data = Geometry.Parse($"M{cX1} {cY1}L{cX2} {cY2}");
        }

        protected override void OnSelectedLayerChanged(object sender)
        {
            // restore pixels to the original (previous) layer
            Target.Frame.SetLayerPixels(2 - Target.SelectedLayer + 1, pixelsBackup);
            // backup the current layer
            Array.Copy(Target.CurrentLayerPixelData, pixelsBackup, 256 * 192);
            if (IsLineSet)
                DrawLine();

        }

        protected override void OnFrameChanged(object sender)
        {
            IsLineSet = false;
            PreviewLine.Visibility = Visibility.Collapsed;
            PreviewLineCapStart.Visibility = Visibility.Collapsed;
            PreviewLineCapEnd.Visibility = Visibility.Collapsed;
        }

        // Bresenham
        public static List<System.Drawing.Point> GetLinePixels(int x0,int y0,int x1,int y1)
        {
            var result = new List<System.Drawing.Point>();
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);    
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;
            while(true)
            {
                result.Add(new System.Drawing.Point(x0, y0));
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if(e2>=dy)
                {
                    err += dy;
                    x0 += sx;
                }
                if(e2<=dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
            return result;
        }        
    }
}
