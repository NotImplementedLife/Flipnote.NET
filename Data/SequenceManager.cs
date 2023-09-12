using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Data
{
    internal class SequenceManager
    {
        public List<SequenceTrack> Tracks { get; } = new List<SequenceTrack>();

        public SequenceManager()
        {
            Tracks.Add(new SequenceTrack());
        }

    }
}
