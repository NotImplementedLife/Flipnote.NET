using FlipnoteDotNet.Extensions;
using FlipnoteDotNet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data
{
    [Serializable]
    public class SequenceTrack
    {
        private ConcurrentList<Sequence> Sequences = new ConcurrentList<Sequence>();
        

        public void AddSequence(Sequence sequence)
        {
            Sequences.Add(sequence);
            sequence.Track = this;
            ElementAdded?.Invoke(this, sequence);          
        }

        public Sequence GetSequenceAtTimestamp(int timestamp)
        {
            return Sequences.Where(_ => timestamp.IsInRange(_.StartFrame, _.EndFrame)).FirstOrDefault();
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
