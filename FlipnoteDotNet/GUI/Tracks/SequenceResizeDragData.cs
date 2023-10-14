using FlipnoteDotNet.Data;

namespace FlipnoteDotNet.GUI.Tracks
{
    internal class SequenceResizeDragData
    {
        public enum _Direction { Left,Right }

        public Sequence Element { get; }

        public _Direction Direction { get; }

        public int Position { get; }

        public SequenceResizeDragData(Sequence element, _Direction direction)
        {
            Element = element;
            Direction = direction;
            Position = Direction == _Direction.Left ? Element.StartFrame : Element.EndFrame;
        }               

        public void Resize(int dx)
        {
            int s = Element.StartFrame;
            int e = Element.EndFrame;

            if(Direction==_Direction.Left)
            {
                s = Position + dx;
            }
            else
            {
                e = Position + dx;
            }

            if (s < 0 || e > 999) return;

            if (s < e) 
            {
                Element.StartFrame = s;
                Element.EndFrame = e;
            }
        }
    }
}
