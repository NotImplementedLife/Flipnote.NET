using FlipnoteDotNet.Data;

namespace FlipnoteDotNet.GUI.Tracks
{
    internal class SequenceMoveDragData
    {
        public Sequence Element { get; }
        public int Start { get; }
        public int End { get; }

        public SequenceMoveDragData(Sequence element)
        {
            Element = element;
            Start = Element.StartFrame;
            End = Element.EndFrame;
        }

        public void Move(int dx)
        {
            int s = Start + dx;
            int e = End + dx;

            if (s < 0 || e > 999) return;

            Element.StartFrame = s;
            Element.EndFrame = e;
        }


    }
}
