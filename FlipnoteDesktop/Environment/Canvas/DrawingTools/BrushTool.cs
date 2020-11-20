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
    public class BrushTool : DrawingTool
    {
        public BrushTool()
        {
            Attached += BrushTool_Attached;
            Detached += BrushTool_Detached;
        }

        Rectangle PreviewRect = new Rectangle() { IsHitTestVisible = false };

        NumericInput SizeInput;
        PatternSelector PatternInput;

        public void BrushTool_Attached(object o)
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
                Text = "Size : ",
                VerticalAlignment=System.Windows.VerticalAlignment.Center,
                Height=21,
                Margin = new System.Windows.Thickness(3, 0, 3, 0)
            });
            Target.ToolOptions.Children.Add(SizeInput);
            Target.ToolOptions.Children.Add(new TextBlock
            {
                Text = "Pattern : ",
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                Height = 21,
                Margin=new System.Windows.Thickness(3,0,3,0)
            });
            Target.ToolOptions.Children.Add(PatternInput);
            Target.ExtensionPanel.Children.Add(PreviewRect);            
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

        public void BrushTool_Detached(object o)
        {
            SizeInput = null;
        }

        public int Size = 1;
        public Pattern Pattern = Patterns.Mono;

        void PutPoint(int x,int y)
        {
            int maxX = x + Size / 2 - 1 + (Size & 1);
            int maxY = y + Size / 2 - 1 + (Size & 1);            
            for (int _x = Math.Max(0,x - Size / 2); _x <= maxX; _x++) 
            {
                for (int _y = Math.Max(0, y - Size / 2); _y <= maxY; _y++) 
                    if (Pattern.GetPixelAt(_x, _y))
                        Target.SetPixel(_x, _y);
            }
            Target.UpdateImage();
        }

        void ErasePoint(int x,int y)
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

        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {           
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                PutPoint(Target.canvasX, Target.canvasY);                
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                ErasePoint(Target.canvasX, Target.canvasY);
            }           
        }
        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            System.Windows.Controls.Canvas.SetLeft(PreviewRect, Target.Zoom * (Target.canvasX-Size/2));
            System.Windows.Controls.Canvas.SetTop(PreviewRect, Target.Zoom * (Target.canvasY - Size / 2));
            PreviewRect.Width = PreviewRect.Height = Size * Target.Zoom;
            PreviewRect.Fill = Brushes.Green;
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                PutPoint(Target.canvasX, Target.canvasY);                
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                ErasePoint(Target.canvasX, Target.canvasY);                
            }                      
        }

        protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
            PreviewRect.Width = PreviewRect.Height = 0;
        }
    }    
}
