using PPMLib.Data;
using System.Drawing;

namespace FlipnoteDotNet.Extensions
{
    public static class FlipnoteExtensions
    {
        public static Color ToColor(this FlipnotePaperColor paperColor)
        {
            if (paperColor == FlipnotePaperColor.White)
                return Constants.Colors.FlipnoteWhite;
            return Constants.Colors.FlipnoteBlack;
        }

        public static Brush ToBrush(this FlipnotePaperColor paperColor) => paperColor.ToColor().GetBrush();

        public static FlipnotePaperColor Invert(this FlipnotePaperColor paperColor)
            => paperColor == FlipnotePaperColor.White ? FlipnotePaperColor.Black : FlipnotePaperColor.White; 

        public static Color ToColor(this FlipnotePen pen, FlipnotePaperColor paperColor)
        {
            switch(pen)
            {
                case FlipnotePen.Red: return Constants.Colors.FlipnoteRed;
                case FlipnotePen.Blue: return Constants.Colors.FlipnoteBlue;
                default: return paperColor.Invert().ToColor();
            }
        }
        public static Brush ToBrush(this FlipnotePen pen, FlipnotePaperColor paperColor) => pen.ToColor(paperColor).GetBrush();
    }
}
