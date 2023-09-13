using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data
{
    public class SequenceTrack
    {
        public class Element
        {
            public Sequence Sequence { get; }
            public int TimestampStart { get; set; }
            public int TimestampEnd { get; set; }
            public SequenceTrack Track { get; internal set; }

            public Element(Sequence sequence, int timestampStart, int timestampEnd)
            {
                Sequence = sequence;
                TimestampStart = timestampStart;
                TimestampEnd = timestampEnd;
            }
        }

        private Dictionary<Sequence, Element> Elements { get; } = new Dictionary<Sequence, Element>();
        public IEnumerable<Element> GetElements() => Elements.Values.AsEnumerable();

        public Element GetElement(Sequence s)
        {
            return Elements.Values.Where(_ => _.Sequence == s).FirstOrDefault();
        }

        public void AddElement(Element elem)
        {
            Elements.Add(elem.Sequence, elem);
            elem.Track = this;
            ElementAdded?.Invoke(this, elem);
        }

        public void AddSequence(Sequence sequence, int timestampStart, int timestampEnd)
        {
            var elem = new Element(sequence, timestampStart, timestampEnd);
            Elements.Add(sequence, elem);
            elem.Track = this;
            ElementAdded?.Invoke(this, elem);
        }

        public void RemoveSequence(Sequence sequence)
        {
            if (!Elements.ContainsKey(sequence))
                return;
            var elem = Elements[sequence];
            elem.Track = null;
            Elements.Remove(sequence);
            ElementRemoved?.Invoke(this, elem);
        }

        public event EventHandler<Element> ElementAdded;
        public event EventHandler<Element> ElementRemoved;
    }
}
