using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlipnoteDesktop.Environment.Canvas
{
    public class BrushTool : DrawingTool
    {       

        void PutPixels(int x,int y)
        {

        }

        protected override void OnMouseDown(object sender, MouseButtonEventArgs e)
        {                
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Target.SetPixel(Target.canvasX, Target.canvasY);
                Target.UpdateImage();                
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Target.ErasePixel(Target.canvasX, Target.canvasY);
                Target.UpdateImage();
            }           
        }
        protected override void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                Target.SetPixel(Target.canvasX, Target.canvasY);
                Target.UpdateImage();               
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Target.ErasePixel(Target.canvasX, Target.canvasY);
                Target.UpdateImage();
            }                      
        }
        protected override void OnMouseUp(object sender, MouseEventArgs e)
        {            
        }
    }
}
