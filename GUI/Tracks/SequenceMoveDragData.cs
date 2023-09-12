using FlipnoteDotNet.Data;

namespace FlipnoteDotNet.GUI.Tracks
{
    internal class SequenceMoveDragData
    {
        public SequenceTrack.Element Element { get; }
        public int Start { get; }
        public int End { get; }

        public SequenceMoveDragData(SequenceTrack.Element element)
        {
            Element = element;
            Start = Element.TimestampStart;
            End = Element.TimestampEnd;
        }

        public void Move(int dx)
        {
            int s = Start + dx;
            int e = End + dx;

            if (s < 0 || e > 999) return;

            Element.TimestampStart = s;
            Element.TimestampEnd = e;
        }


    }
}
