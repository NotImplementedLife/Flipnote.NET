using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FlipnoteDesktop.Environment.Canvas
{
    public class PenTool : DrawingTool
    {
        public PenTool()
        {
            Attached += PenTool_Attached;
            Detached += PenTool_Detached;
        }

        Rectangle PreviewRect = new Rectangle() { IsHitTestVisible = false };

        public void PenTool_Attached(object o)
        {
            Target.ExtensionPanel.Children.Add(PreviewRect);
            Target.ExtensionPanel.ClipToBounds = true;
        }

        public void PenTool_Detached(object o)
        {

        }

        public int Size = 5;
        public Pattern Pattern = Patterns.Dotted1;

        void PutPoint(int x,int y)
        {
            for (int _x = Math.Max(0,x - Size / 2); _x <= x + Size / 2; _x++) 
            {
                for (int _y = Math.Max(0,y - Size / 2); _y <= y + Size / 2; _y++) 
                    if (Pattern.GetPixelAt(_x, _y))
                        Target.SetPixel(_x, _y);
            }
            Target.UpdateImage();
        }

        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {           
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                PutPoint(Target.canvasX, Target.canvasY);
                //Target.SetPixel(Target.canvasX, Target.canvasY);
                //Target.UpdateImage();                
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Target.ErasePixel(Target.canvasX, Target.canvasY);
                Target.UpdateImage();
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
                //Target.SetPixel(Target.canvasX, Target.canvasY);
                //Target.UpdateImage();               
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {                
                Target.ErasePixel(Target.canvasX, Target.canvasY);
                Target.UpdateImage();
            }                      
        }

        protected override void OnMouseLeave(object sender, MouseEventArgs e)
        {
            PreviewRect.Width = PreviewRect.Height = 0;
        }
    }    
}
