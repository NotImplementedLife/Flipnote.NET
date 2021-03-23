using FlipnoteDotNet.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FlipnoteDotNet.Environment.Canvas.DrawingTools
{
    internal class PenTool : DrawingTool
    {
        public PenTool()
        {
            Attached += PenTool_Attached;
            Detached += PenTool_Detached;
            BuildPreviewShape();
        }

        Viewbox PreviewShape = new Viewbox() { IsHitTestVisible = false };        
        PenPatternSelector PatternInput;

        public void PenTool_Attached(object o)
        {           
            PatternInput = new PenPatternSelector
            {
                Height = 30,
                Width = 50,
            };
            PatternInput.ValueChanged += PatternInput_ValueChanged;
            
            Target.ToolOptions.Children.Add(new TextBlock
            {
                Text = "Pattern : ",
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = 21,
                Margin = new System.Windows.Thickness(3, 0, 3, 0)
            });
            Target.ToolOptions.Children.Add(PatternInput);
            Target.ExtensionPanel.Children.Add(PreviewShape);
            Target.ExtensionPanel.ClipToBounds = true;
        }

        private void PatternInput_ValueChanged(object o)
        {
            Pattern = PatternInput.Value;
            BuildPreviewShape();
        }        

        public void PenTool_Detached(object o)
        {
            
        }

        public Pattern Pattern = PenPatterns.Mono;

        void PutPoint(int x, int y, bool updateImage = true)
        {
            int szX = Pattern.Cols;
            int szY = Pattern.Rows;                        
            for (int _x = 0; _x < szX; _x++)
                for (int _y = 0; _y < szY; _y++)
                {
                    if (Pattern.GetPixelAt(_x, _y))
                    {
                        Target.SetPixel(Math.Max(0, x - szX / 2) + _x, Math.Max(0, y - szY / 2) + _y);
                    }
                }
            if (updateImage)
                Target.UpdateImage();
        }

        void PutLine(int x0, int y0, int x1, int y1)
        {
            var pts = LineTool.GetLinePixels(x0, y0, x1, y1);
            if (Pattern.ContinuousDraw) 
            {
                int lstX = x0, lstY = y0;
                for (int i = 1; i < pts.Count; i++)
                {
                    var dx = pts[i].X - x0;
                    var dy = pts[i].Y - y0;
                    if (dx == 0 || dy == 0)
                    {
                        PutPoint(pts[i].X, pts[i].Y, false);
                    }
                    else
                    {
                        PutPoint(x0 + dx, y0, false);
                        PutPoint(pts[i].X, pts[i].Y, false);
                    }
                    x0 = pts[i].X;
                    y0 = pts[i].Y;
                }
            }
            else
            {
                for (int i = 1; i < pts.Count; i++)
                    PutPoint(pts[i].X, pts[i].Y, false);                             
            }
            Target.UpdateImage();
        }

        void ErasePoint(int x, int y, bool updateImage = true)
        {
            int szX = Pattern.Cols;
            int szY = Pattern.Rows;
            for (int _x = 0; _x < szX; _x++)
                for (int _y = 0; _y < szY; _y++)
                {
                    if (Pattern.GetPixelAt(_x, _y))
                    {
                        Target.ErasePixel(Math.Max(0, x - szX / 2) + _x, Math.Max(0, y - szY / 2) + _y);
                    }
                }
            if (updateImage)
                Target.UpdateImage();
        }

        void EraseLine(int x0, int y0, int x1, int y1)
        {
            var pts = LineTool.GetLinePixels(x0, y0, x1, y1);
            for (int i = 1; i < pts.Count; i++)
                ErasePoint(pts[i].X, pts[i].Y, false);
            Target.UpdateImage();
        }

        int lastX = -1, lastY = -1;
        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                PutPoint(Target.canvasX, Target.canvasY);
                lastX = Target.canvasX;
                lastY = Target.canvasY;
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                ErasePoint(Target.canvasX, Target.canvasY);
                lastX = Target.canvasX;
                lastY = Target.canvasY;
            }
        }
        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Canvas.SetLeft(PreviewShape, Target.Zoom * (Target.canvasX - Pattern.Cols / 2));
            System.Windows.Controls.Canvas.SetTop(PreviewShape, Target.Zoom * (Target.canvasY - Pattern.Rows / 2));
            PreviewShape.Width = Pattern.Cols * Target.Zoom;
            PreviewShape.Height = Pattern.Rows * Target.Zoom;            
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (lastX >= 0)
                    PutLine(lastX, lastY, Target.canvasX, Target.canvasY);
                else
                    PutPoint(Target.canvasX, Target.canvasY);
                lastX = Target.canvasX;
                lastY = Target.canvasY;
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                if (lastX >= 0)
                    EraseLine(lastX, lastY, Target.canvasX, Target.canvasY);
                else
                    ErasePoint(Target.canvasX, Target.canvasY);
                lastX = Target.canvasX;
                lastY = Target.canvasY;
            }
        }

        protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
            PreviewShape.Width = PreviewShape.Height = 0;
            lastX = -1;
        }

        private void BuildPreviewShape()
        {            
            var canvas= new System.Windows.Controls.Canvas()
            {
                Width = 5 * Pattern.Cols,
                Height = 5 * Pattern.Rows
            };           
            for (int x = 0; x < Pattern.Cols; x++)
                for (int y = 0; y < Pattern.Rows; y++)
                    if (Pattern.GetPixelAt(x, y))
                    {
                        var sq = new Rectangle()
                        {
                            Width=5,
                            Height=5,
                            Fill=Brushes.Green
                        };
                        System.Windows.Controls.Canvas.SetLeft(sq, 5 * x);
                        System.Windows.Controls.Canvas.SetTop(sq, 5 * y);
                        canvas.Children.Add(sq);
                    }
            PreviewShape.Child = canvas;            
            System.Windows.Controls.Canvas.SetLeft(PreviewShape, 200 * (-Pattern.Cols));
            System.Windows.Controls.Canvas.SetTop(PreviewShape, 200 * (-Pattern.Rows));            
        }
    }
}
