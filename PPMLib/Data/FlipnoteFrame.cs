using System;
using System.Collections.Generic;
using System.Text;

namespace PPMLib.Data
{
    public class FlipnoteFrame
    {        
        public FlipnotePaperColor PaperColor { get; set; } = FlipnotePaperColor.White;
        public FlipnoteFrameLayer Layer1 { get; } = new FlipnoteFrameLayer();
        public FlipnoteFrameLayer Layer2 { get; } = new FlipnoteFrameLayer();

        public FlipnotePen Pen1
        {
            get => Layer1.Pen;
            set => Layer1.Pen = value;
        }

        public FlipnotePen Pen2
        {
            get => Layer2.Pen;
            set => Layer2.Pen = value;
        }

        public FlipnoteFrame(FlipnotePaperColor paperColor, FlipnotePen pen1, FlipnotePen pen2)
        {
            PaperColor = paperColor;
            Pen1 = pen1;
            Pen2 = pen2;
        }

        public FlipnoteFrame()
        {
        }
    }
}
