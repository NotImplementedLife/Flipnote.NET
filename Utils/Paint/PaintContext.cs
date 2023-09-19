using PPMLib.Data;

namespace FlipnoteDotNet.Utils.Paint
{
    public class PaintContext
    {
        public int PenValue { get; set; }
        public FlipnotePen Pen1 { get; set; } = FlipnotePen.PaperInverse;
        public FlipnotePen Pen2 { get; set; } = FlipnotePen.Red;
        

    }
}
