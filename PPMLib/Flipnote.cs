using PPMLib.Data;
using System;
using System.Collections.Generic;

namespace PPMLib
{
    public class Flipnote
    {
        public bool Locked { get; set; }
        public FlipnoteAuthor RootAuthor { get; set; }
        public FlipnoteAuthor ParentAuthor { get; set; }
        public FlipnoteAuthor CurrentAuthor { get; set; }
        public FlipnoteFilename ParentFilename { get; set; }
        public FlipnoteFilename CurrentFilename { get; set; }
        public RootFilenameFragment RootFilenameFragment { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int ThumbnailFrameId { get; set; }
        public FlipnoteThumbnail Thumbnail { get; set; }
        public List<FlipnoteFilename> Frames { get; } = new List<FlipnoteFilename>();
        public bool LoopPlayback { get; set; }
        public bool Layer1Hidden { get; set; }
        public bool Layer2Hidden { get; set; }

        public FlipnoteSoundtrack Bgm { get; }
        public FlipnoteSoundtrack Sfx1 { get; }
        public FlipnoteSoundtrack Sfx2 { get; }
        public FlipnoteSoundtrack Sfx3 { get; }
    }
}
