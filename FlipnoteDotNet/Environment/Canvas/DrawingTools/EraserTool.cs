using FlipnoteDotNet.Controls;
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

namespace FlipnoteDotNet.Environment.Canvas.DrawingTools
{
    internal class EraserTool : DrawingTool
    {
        public EraserTool()
        {
            Attached += EraserTool_Attached;
            Detached += BrushTool_Detached;
        }

        Rectangle PreviewRect = new Rectangle() { IsHitTestVisible = false };

        NumericInput SizeInput;

        public void EraserTool_Attached(object o)
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

            Target.ToolOptions.Children.Add(new TextBlock
            {
                Text = "Size : ",
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = 21,
                Margin = new System.Windows.Thickness(3, 0, 3, 0)
            });
            Target.ToolOptions.Children.Add(SizeInput);
            
            Target.ExtensionPanel.Children.Add(PreviewRect);
            Target.ExtensionPanel.ClipToBounds = true;
        }       

        private void SizeInput_ValueChanged(object o)
        {
            Size = SizeInput.Value;
        }

        public void BrushTool_Detached(object o)
        {
            SizeInput = null;
        }

        public int Size = 1;

        void ErasePoint(int x, int y, bool updateImage = true)
        {
            int maxX = x + Size / 2 - 1 + (Size & 1);
            int maxY = y + Size / 2 - 1 + (Size & 1);
            for (int _x = Math.Max(0, x - Size / 2); _x <= maxX; _x++)
            {
                for (int _y = Math.Max(0, y - Size / 2); _y <= maxY; _y++)
                    Target.ErasePixel(_x, _y);
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

        int lastX, lastY;
        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                ErasePoint(Target.canvasX, Target.canvasY);
                lastX = Target.canvasX;
                lastY = Target.canvasY;
            }            
        }
        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Canvas.SetLeft(PreviewRect, Target.Zoom * (Target.canvasX - Size / 2));
            System.Windows.Controls.Canvas.SetTop(PreviewRect, Target.Zoom * (Target.canvasY - Size / 2));
            PreviewRect.Width = PreviewRect.Height = Size * Target.Zoom;
            PreviewRect.StrokeThickness = 1;
            PreviewRect.Stroke = Brushes.Green;
            PreviewRect.Fill = Brushes.White;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
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
            PreviewRect.Width = PreviewRect.Height = 0;
            lastX = -1;
        }
    }
}
