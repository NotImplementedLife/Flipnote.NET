using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data
{
    public class SequenceTrack
    {       
        private List<Sequence> Sequences = new List<Sequence>();

        public void AddSequence(Sequence sequence)
        {
            Sequences.Add(sequence);
            sequence.Track = this;
            ElementAdded?.Invoke(this, sequence);          
        }

        public IEnumerable<Sequence> GetSequences() => Sequences.AsEnumerable();

        public void RemoveSequence(Sequence sequence)
        {
            if(Sequences.Remove(sequence))
            {
                sequence.Track = null;
                ElementRemoved?.Invoke(this, sequence);
            }           
        }

        public event EventHandler<Sequence> ElementAdded;
        public event EventHandler<Sequence> ElementRemoved;
    }
}
