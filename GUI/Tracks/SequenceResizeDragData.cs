using FlipnoteDotNet.Data;

namespace FlipnoteDotNet.GUI.Tracks
{
    internal class SequenceResizeDragData
    {
        public enum _Direction { Left,Right }

        public SequenceTrack.Element Element { get; }

        public _Direction Direction { get; }

        public int Position { get; }

        public SequenceResizeDragData(SequenceTrack.Element element, _Direction direction)
        {
            Element = element;
            Direction = direction;
            Position = Direction == _Direction.Left ? Element.TimestampStart : Element.TimestampEnd;
        }               

        public void Resize(int dx)
        {
            int s = Element.TimestampStart;
            int e = Element.TimestampEnd;

            if(Direction==_Direction.Left)
            {
                s = Position + dx;
            }
            else
            {
                e = Position + dx;
            }

            if (s + 1 < e)
            {
                Element.TimestampStart = s;
                Element.TimestampEnd = e;
            }
        }
    }
}
