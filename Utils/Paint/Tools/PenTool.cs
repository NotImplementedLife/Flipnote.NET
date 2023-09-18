using FlipnoteDotNet.Attributes;
using FlipnoteDotNet.Utils.Paint.Operations;

namespace FlipnoteDotNet.Utils.Paint.Tools
{
    [PaintTool("Pen", nameof(Properties.Resources.ic_paint_pen))]
    public class PenTool : IPaintTool
    {
        public IPaintOperation CreateOperation()
        {
            return new PenOperation();
        }
    }
}
