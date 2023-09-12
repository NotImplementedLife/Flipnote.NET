using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Data
{
    internal class SequenceTrack
    {
        public class Element
        {
            public int Timestamp { get; set; }
            public Sequence Sequence { get; }

            public Element(int timestamp, Sequence sequence)
            {
                Timestamp = timestamp;
                Sequence = sequence;
            }
        }
        private List<Element> Elements { get; } = new List<Element>();




    }
}
