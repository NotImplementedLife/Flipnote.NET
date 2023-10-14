using FlipnoteDotNet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data
{
    [Serializable]
    public class SequenceManager
    {
        private ConcurrentList<SequenceTrack> Tracks { get; } = new ConcurrentList<SequenceTrack>();

        public List<SequenceTrack> GetTracks() => Tracks.ToSimpleList();

        public SequenceManager() : this(5) { }

        public SequenceManager(int tracksCount)
        {
            for (int i = 0; i < tracksCount; i++)
                AddNewTrack();
        }     

        public void AddNewTrack()
        {
            var track=new SequenceTrack();            
            track.ElementAdded += Track_ElementAdded;
            track.ElementRemoved += Track_ElementRemoved;
            Tracks.Add(track);
        }        

        public SequenceTrack GetTrack(int index) => Tracks[index];
        public int TracksCount => Tracks.Count;

        private void Track_ElementRemoved(object sender, Sequence e)
        {
            ElementRemoved?.Invoke(this, sender as SequenceTrack, e);
        }

        private void Track_ElementAdded(object sender, Sequence e)
        {
            ElementAdded?.Invoke(this, sender as SequenceTrack, e);
        }

        public delegate void OnElementAdded(SequenceManager sender, SequenceTrack track, Sequence e);
        public delegate void OnElementRemoved(SequenceManager sender, SequenceTrack track, Sequence e);

        public event OnElementAdded ElementAdded;
        public event OnElementRemoved ElementRemoved;

    }
}
