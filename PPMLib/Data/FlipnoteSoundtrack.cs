using System;
using System.Collections.Generic;
using System.Text;

namespace PPMLib.Data
{
    public class FlipnoteSoundtrack
    {
        public short[] SoundPCM16 { get; set; }

        public FlipnoteSoundtrack(short[] soundPCM16)
        {
            SoundPCM16 = soundPCM16;
        }
    }
}
