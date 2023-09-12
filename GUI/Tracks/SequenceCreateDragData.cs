using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.GUI.Tracks
{
    internal class SequenceCreateDragData
    {
        public int StartX { get; }        
        public int EndX { get; set; }        
        public int TrackId { get; }

        public SequenceCreateDragData(int startX, int trackId)
        {
            StartX = startX;
            EndX = startX;
            TrackId = trackId;
        }
    }
}
