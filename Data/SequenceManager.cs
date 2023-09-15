using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Data
{
    internal class SequenceManager
    {
        private List<SequenceTrack> Tracks { get; } = new List<SequenceTrack>();

        public SequenceManager(int tracksCount=0)
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
